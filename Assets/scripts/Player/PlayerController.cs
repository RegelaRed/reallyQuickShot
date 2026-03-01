using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Central player controller coordinating input, context updates,
/// movement state machine, and camera state machine.
/// <para/>
/// Responsibilities:
/// • Collect player input and create input snapshots
/// • Maintain PlayerContext data
/// • Run movement and camera state machines
/// • Update PlayerMotor physics
/// • Provide scene references to states
/// </summary>
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

    [Header("Scripts")]
    [SerializeField] private PlayerInputHandler _input;
    private PlayerInputBuffer _inputBuffer;
    [SerializeField] private PlayerMotor _playerMotor;

    private PlayerContext _playerContext;
    // private variables
    private bool _lastAttackPressed = false;

    // ─────────────── State Machines ─────────────── 
    /// <summary>
    /// Movement hierarchical state machine.
    /// Root states: Grounded, Falling, Jump, Dash
    /// Substates: Idle, Walk, Sprint
    /// </summary>
    private PlayerBaseState _currentMovementState;
    private PlayerStateFactory _movementFactory;

    /// <summary>
    /// Camera state machine.
    /// States: MainCamera, AimCamera
    /// </summary>
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

    /// <summary>
    /// Initializes player systems and state machines.
    /// <para/>
    /// Order is important:
    /// 1. Input & Motor setup
    /// 2. Context creation
    /// 3. Variable pre-calculation
    /// 4. Movement FSM initialization
    /// 5. Camera FSM initialization
    /// </summary>
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

    /// <summary>
    /// Main update loop.
    /// <para/>
    /// Execution order:
    /// 1. Create input snapshot
    /// 2. Update timers and input buffer
    /// 3. Update movement FSM
    /// 4. Apply movement physics
    /// 5. Update camera FSM
    /// </summary>
    private void Update()
    {

        _playerContext.Input = CreateInputSnapshot();
        _playerContext.IsGrounded = _characterController.isGrounded;
        _playerContext.DeltaTime = Time.deltaTime;

        _playerContext.UpdateAbilityTimers();

        _inputBuffer.Register(_playerContext.Input);
        _inputBuffer.Tick(Time.deltaTime);

        //Movement State machine
        _currentMovementState.UpdateStates(_playerContext);
        var nextMove = _currentMovementState.CheckSwitchState(_playerContext);
        if (nextMove != null)
            _currentMovementState.SwitchStates(nextMove, _playerContext, this);


        _playerMotor.TickPhysics(_playerContext);

        //Camera State machine
        _currentCameraState.UpdateStates(_playerContext);
        var nextCam = _currentCameraState.CheckSwitchState(_playerContext);
        if (nextCam != null)
            _currentCameraState.SwitchStates(nextCam, _playerContext);
    }

    #endregion
    #region Helper Functions
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public PlayerInputSnapshot CreateInputSnapshot()
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
            DashDirection = GetDashDirection(),

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

    /// <summary>
    /// Applies impulse force to rigidbodies when the player collides with them.
    /// Only affects non-kinematic rigidbodies.
    /// </summary>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * _playerVariables.pushForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Pre-calculates physics constants used by movement states.
    /// <para/>
    /// Jump calculations use kinematic equations:
    /// gravity = -2h / t²
    /// velocity = 2h / t
    /// <para/>
    /// Dash calculations determine velocities required to reach
    /// configured distance and height within dash duration.
    /// </summary>
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
        context.InitialDashVerticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(context.JumpGravity) * context.Variables.dashApexHeight);
        context.InitialDashHorizontalVelocity = context.Variables.dashDistance / context.Variables.dashDuration;

        context.CurrentGravity = context.JumpGravity;
    }

    /// <summary>
    /// Returns the direction used for dashing.
    /// <para/>
    /// Aim mode:
    ///     Dash follows camera orientation.
    /// Normal mode:
    ///     Dash follows player facing direction.
    /// </summary>
    /// <returns>Normalized dash direction vector</returns>
    public Vector3 GetDashDirection()
    {
        if (_currentCameraState is PlayerAimCamera)
            return _orientation.forward;
        return _faceDirection.forward;
    }

    // ─────────────── Cursor ─────────────── 
    /// <summary> Hides and Locks mouse </summary>
    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary> Unhides and Unlockls mouse  </summary>
    public void UnhideMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}