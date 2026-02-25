using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Player Controller Manages Player Behaviour
/// Inputs, Scene References, Player Movement State Machine, Player Camera State Machine
/// 
/// </summary>
/// <remarks>
/// 
/// </remarks>
public class PlayerController : MonoBehaviour
{
    #region References
    // ─────────────── References ─────────────── 

    [Header("Scene References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCameraPosition;
    [SerializeField] private Transform _faceDirection;

    [Header("Cameras")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _aimCamera;

    [Header("Player Data")]
    [SerializeField] private PlayerVariables _playerVariables;
    [SerializeField] private LayerMask groundMask;

    [Header("Scripts")]
    [SerializeField] private PlayerInputHandler _input;
    private PlayerInputBuffer _inputBuffer;
    [SerializeField] private PlayerMotor _playerMotor;

    private PlayerContext _playerContext;
    // private variables
    private bool _lastAttackPressed = false;

    // ─────────────── State Machines ─────────────── 
    private PlayerBaseState _currentMovementState;
    private PlayerStateFactory _movementFactory;

    private PlayerCameraBaseState _currentCameraState;
    private PlayerCameraStateFactory _cameraFactory;

    #endregion
    #region Getters/Setters
    // ─────────────── Properties ─────────────── 

    public CharacterController Controller => _characterController;
    public Transform Orientation => _orientation;
    public Transform PlayerCameraPosition => _playerCameraPosition;
    public Transform FaceDirection => _faceDirection;

    public GameObject MainCamera => _mainCamera;
    public GameObject AimCamera => _aimCamera;


    public PlayerBaseState CurrentMovementState
    {
        get => _currentMovementState;
        set => _currentMovementState = value;
    }

    public PlayerCameraBaseState CurrentCameraState
    {
        get => _currentCameraState;
        set => _currentCameraState = value;
    }
    #endregion
    #region Update Methods
    // ─────────────── Unity Lifecycle ─────────────── 

    private void Awake()
    {
        HideMouse();

        _input ??= GetComponent<PlayerInputHandler>() ?? this.AddComponent<PlayerInputHandler>();
        _playerMotor ??= GetComponent<PlayerMotor>();
        if (_playerMotor == null)
            Debug.Log("No Playermotor component");

        //Context
        _inputBuffer = new PlayerInputBuffer(_playerVariables.jumpBufferTime, _playerVariables.dashBufferTimer);
        _playerContext = new PlayerContext()
        {
            Variables = _playerVariables,
            InputBuffer = _inputBuffer
        };

        //Variables Initialization
        SetupVariables(_playerContext);

        //Movement
        _movementFactory = new PlayerStateFactory(_playerMotor);
        _currentMovementState = _movementFactory.Grounded();
        _currentMovementState.EnterState(_playerContext);

        //Camera
        _cameraFactory = new PlayerCameraStateFactory(this, _playerMotor);
        _currentCameraState = _cameraFactory.MainCamera();
        _currentCameraState.EnterState(_playerContext);
    }
    PlayerBaseState lastState;
    PlayerBaseState activeState;
    private void Update()
    {
        activeState = _currentMovementState.CurrentSubState ?? _currentMovementState;
        if (lastState != activeState)
        {
            Debug.Log("------------------------------------"
                + System.Environment.NewLine
                + $"current active state {activeState}");

            Debug.Log($"current movement speed {_playerMotor.CurrentSpeed}");
            Debug.Log($"current movement gravity {_playerMotor.Gravity}");
            lastState = activeState;
        }

        _playerContext.Input = CreateSnapshot();

        _playerContext.DeltaTime = Time.deltaTime;
        _playerContext.IsGrounded = _characterController.isGrounded;

        _playerContext.DashDirection = GetDashDirection();
        _playerContext.AbilityTimers();

        _inputBuffer.Register(_playerContext.Input);
        _inputBuffer.Tick(Time.deltaTime);

        //Movement State machine
        _currentMovementState.UpdateStates(_playerContext);
        var nextMove = _currentMovementState.CheckSwitchState(_playerContext);
        if (nextMove != CurrentMovementState)
            _currentMovementState.SwitchStates(nextMove, _playerContext, this);


        _playerMotor.UpdatePhysics(_playerContext);

        //Camera State machine
        _currentCameraState.UpdateStates(_playerContext);
        var nextCam = _currentCameraState.CheckSwitchState(_playerContext);
        if (nextCam != _currentCameraState)
            _currentCameraState.SwitchStates(nextCam, _playerContext);
    }

    private void SwitchCameraState(PlayerCameraBaseState newState)
    {
        _currentCameraState.ExitState(_playerContext);

        _currentCameraState = newState;

        _currentCameraState.EnterState(_playerContext);
    }
    #endregion
    #region Helper Functions
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public PlayerInputSnapshot CreateSnapshot()
    {
        PlayerInputSnapshot snapshot = new PlayerInputSnapshot
        {
            Move = _input.CurrentMovementInput,
            Look = _input.CurrentLookInput,
            MovePressed = _input.IsMovementPressed,

            SprintPressed = _input.IsSprintPressed,
            SprintToggle = _input.SprintToggle,

            JumpPressed = _input.IsJumpPressedThisFrame,
            JumpHeld = _input.IsJumpPressed,

            DashPressed = _input.IsDashPressedThisFrame,
            DashHeld = _input.IsDashPressed,

            AttackHeld = _input.AttackHeld,
            AttackPressed = _input.AttackPressed,
            AttackReleased = _lastAttackPressed && !_input.AttackHeld,

            ReloadPressed = _input.ReloadPressed,

            AmmoPrevious = _input.WeaponPrevious,
            AmmoNext = _input.WeaponNext,
            SwitchWeapon = _input.SwitchAmmoPressed,
            AimMode = _input.AimToggle
        };
        _lastAttackPressed = _input.AttackHeld;
        return snapshot;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * _playerVariables.pushForce, ForceMode.Impulse);
        }
    }

    private void SetupVariables(PlayerContext context)
    {

        // ------------ Jump ------------
        float jumpTimeToApex = context.Variables.maxJumpDuration * 0.5f;
        context.JumpTimeToApex = jumpTimeToApex;
        context.JumpGravity = -2f * context.Variables.maxJumpHeight / (jumpTimeToApex * jumpTimeToApex);
        context.InitialJumpVerticalVelocity = 2f * context.Variables.maxJumpHeight / jumpTimeToApex;

        // ------------ Dash ------------
        float dashApexTime = context.Variables.dashDuration;

        context.DashCharges = context.Variables.maxDashCharges;
        context.DashGravity = context.Variables.dashApexHeight / (dashApexTime * dashApexTime);
        context.InitialDashVerticalVelocity = Mathf.Sqrt(2f * context.JumpGravity * context.Variables.dashApexHeight);
        context.InitialDashHorizontalVelocity = context.Variables.dashDistance / context.Variables.dashDuration;

        context.CurrentGravity = context.JumpGravity;
    }

    /// <summary>
    /// Get the Forward Direction of the Player
    /// </summary>
    /// <returns> Vector3 Player Forward </returns>
    public Vector3 GetDashDirection()
    {
        if (_currentCameraState is PlayerAimCamera)
            return _orientation.forward;
        return _faceDirection.forward;
    }

    // ─────────────── Cursor ─────────────── 
    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnhideMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}