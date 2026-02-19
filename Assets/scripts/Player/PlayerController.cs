using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region erences
    // ─────────────── erences ─────────────── 

    [Header("Scene erences")]
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

        _input ??= GetComponent<PlayerInputHandler>();
        _playerMotor ??= GetComponent<PlayerMotor>();

        //Context
        _inputBuffer = new PlayerInputBuffer(_playerVariables.jumpBufferTime, _playerVariables.dashBufferTimer);
        _playerContext = new PlayerContext()
        {
            PlayerMotor = _playerMotor,
            Variables = _playerVariables,
            InputBuffer = _inputBuffer
        };

        SetupJumpVariables(_playerContext);
        SetupDashVariables(_playerContext);

        //Movement
        _movementFactory = new PlayerStateFactory(this);
        _currentMovementState = _movementFactory.Grounded();
        _currentMovementState.EnterState(_playerContext);

        //Camera
        _cameraFactory = new PlayerCameraStateFactory(this);
        _currentCameraState = _cameraFactory.MainCamera();
        _currentCameraState.EnterState(_playerContext);
    }
    private void Update()
    {
        _playerContext.Input = _input.CreateSnapshot();

        _playerContext.DeltaTime = Time.deltaTime;
        _playerContext.IsGrounded = _characterController.isGrounded;

        _playerContext.DashDirection = GetDashDirection();
        _playerContext.AbilityTimers();

        _inputBuffer.Register(_playerContext.Input);
        _inputBuffer.Tick(Time.deltaTime);

        _currentMovementState.UpdateStates(_playerContext);

        _playerMotor.UpdatePhysics(_playerContext);

        _currentCameraState.UpdateStates(_playerContext);
    }
    #endregion
    #region Helper Functions
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * _playerVariables.pushForce, ForceMode.Impulse);
        }
    }

    // ─────────────── Jump ─────────────── 
    /// <summary>
    /// Initialize jump variables 
    /// </summary>
    /// <param name="context"> Player Context to be Modified </param>
    private void SetupJumpVariables(PlayerContext context)
    {
        float timeToApex = context.Variables.maxJumpTime * 0.5f;

        context.JumpGravity = -2f * context.Variables.maxJumpHeight / (timeToApex * timeToApex);
        context.InitialJumpVelocity = 2f * context.Variables.maxJumpHeight / timeToApex;
    }

    // ─────────────── Dash ─────────────── 

    /// <summary>
    /// Initialize Dash variables
    /// </summary>
    /// <param name="context"> Player Context to be Modified </param>
    private void SetupDashVariables(PlayerContext context)
    {
        context.DashCharges = context.Variables.maxDashCharges;

        float timeToApex = context.Variables.dashDuration * 0.5f;

        context.DashGravity = -2f * context.Variables.samllDashJumpHeight / (timeToApex * timeToApex);
        context.InitialDashVelocity = 2f * context.Variables.samllDashJumpHeight / timeToApex;
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