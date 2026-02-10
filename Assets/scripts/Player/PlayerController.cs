using System.Collections;
using UnityEngine;

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

    // ─────────────── State Machines ─────────────── 

    private PlayerBaseState _currentMovementState;
    private PlayerStateFactory _movementFactory;

    private PlayerCameraBaseState _currentCameraState;
    private PlayerCameraStateFactory _cameraFactory;

    private Quaternion _currentCameraRotation;


    // ─────────────── Jump ─────────────── 
    //delete
    private float _cyoteTimer;
    //keep
    private float _initialJumpVelocity;
    private float _jumpGravity;


    // ─────────────── Dash ─────────────── 

    //delete
    private float _dashIntervalTimer;
    private float _dashRegenTimer;
    private int _currentDashCharges;
    //keep 
    private float _initialDashVelocity;
    private float _dashGravity;

    #endregion
    #region Getters/Setters
    // ─────────────── Properties ─────────────── 

    public CharacterController Controller => _characterController;
    public Transform Orientation => _orientation;
    public Transform PlayerCameraPosition => _playerCameraPosition;
    public Transform FaceDirection => _faceDirection;

    public GameObject MainCamera => _mainCamera;
    public GameObject AimCamera => _aimCamera;
    public PlayerVariables Variables => _playerVariables;

    public PlayerInputHandler Input => _input;
    public PlayerMotor PlayerMotor => _playerMotor;

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

    public Quaternion CurrentCameraRotation
    {
        get => _currentCameraRotation;
        set => _currentCameraRotation = value;
    }


    // ─────────────── Jump Properties ─────────────── 

    public float InitialJumpVelocity => _initialJumpVelocity;
    public float JumpGravity => _jumpGravity;

    public float TimeLeftOnGround
    {
        get => _cyoteTimer;
        set => _cyoteTimer = value;
    }

    public bool CyoteTrue => _cyoteTimer < 0;


    // ─────────────── Dash Properties ─────────────── 

    public float DashCooldownTimer
    {
        get => _dashIntervalTimer;
        set => _dashIntervalTimer = value;
    }

    public int AvalableDashCharges => _currentDashCharges;

    public bool CanDash =>
        _dashIntervalTimer <= 0f && _currentDashCharges > 0;

    public float InitialDashVelocity => _initialDashVelocity;
    public float DashGravity => _dashGravity;


    // ─────────────── Ground ─────────────── 

    public bool IsOnGround => Controller.isGrounded;

    #endregion
    #region Update Methods
    // ─────────────── Unity Lifecycle ─────────────── 

    private void Awake()
    {
        HideMouse();
        //Context
        _playerContext = new PlayerContext();

        _input ??= GetComponent<PlayerInputHandler>();
        _inputBuffer = new PlayerInputBuffer(_playerVariables.jumpBufferTime, _playerVariables.dashBufferTimer);

        _playerMotor ??= GetComponent<PlayerMotor>();

        //Movement
        _movementFactory = new PlayerStateFactory(this);
        _currentMovementState = _movementFactory.Grounded();
        _currentMovementState.EnterState(ref _playerContext);

        //Camera
        _cameraFactory = new PlayerCameraStateFactory(this);
        _currentCameraState = _cameraFactory.MainCamera();
        _currentCameraState.EnterState();


        SetupJumpVariables();
        SetupDashVariables();
    }
    private void Update()
    {
        UpdateCoyoteTimer();
        UpdateDashInterval();
        UpdateDashRegen();

        _playerContext.Input = _input.CreateSnapshot();

        _inputBuffer.Register(_playerContext.Input);
        _inputBuffer.Tick(Time.deltaTime);

        _playerContext.InputBuffer = _inputBuffer;

        _currentMovementState.UpdateStates(ref _playerContext);

        _playerMotor.UpdatePhysics(_playerContext);
        _currentCameraState.UpdateStates(_playerContext);
    }
    #endregion
    #region Helper Functions
    // ─────────────── Jump ─────────────── 
    private void UpdateCoyoteTimer()
    {
        if (_cyoteTimer > 0f)
            _cyoteTimer -= Time.deltaTime;
    }
    private void SetupJumpVariables()
    {
        float timeToApex = _playerVariables.maxJumpTime * 0.5f;

        _jumpGravity = -2f * _playerVariables.maxJumpHeight / (timeToApex * timeToApex);
        _initialJumpVelocity = 2f * _playerVariables.maxJumpHeight / timeToApex;
    }

    // ─────────────── Dash ─────────────── 

    private void SetupDashVariables()
    {
        _currentDashCharges = _playerVariables.maxDashCharges;

        float timeToApex = Variables.dashDuration * 0.5f;

        _dashGravity = -2f * Variables.samllDashJumpHeight / (timeToApex * timeToApex);
        _initialDashVelocity = 2f * Variables.samllDashJumpHeight / timeToApex;
    }
    public void DashConsume()
    {
        _currentDashCharges--;
        _dashIntervalTimer = Variables.dashInterval;
        _dashRegenTimer = Variables.dashRegenTime;
    }
    private void UpdateDashInterval()
    {
        if (_dashIntervalTimer > 0f)
            _dashIntervalTimer -= Time.deltaTime;
    }

    private void UpdateDashRegen()
    {
        if (_currentDashCharges >= Variables.maxDashCharges)
            return;

        if (_dashRegenTimer > 0f)
        {
            _dashRegenTimer -= Time.deltaTime;
            return;
        }

        _currentDashCharges++;
        _currentDashCharges = Mathf.Min(
            _currentDashCharges,
            Variables.maxDashCharges
        );

        _dashRegenTimer = Variables.dashRegenTime;
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

    // ─────────────── Utilities ─────────────── 

    public Coroutine RunCorutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
    #endregion
}