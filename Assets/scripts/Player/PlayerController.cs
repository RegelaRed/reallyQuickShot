using UnityEngine;
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCameraPosition;
    [SerializeField] private Transform _faceDirection;

    //Cinemachine Camera
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _aimCamera;
    [SerializeField] private PlayerVariables _playerVariables;

    //script references
    [SerializeField] private PlayerInputHandler _input;
    private PlayerBaseState _currentMovementState;
    private PlayerStateFactory _movementFactory;

    private PlayerCameraBaseState _currentCameraState;
    private PlayerCameraStateFactory _cameraFactory;

    [SerializeField] private PlayerMotor _playerMotor;

    //runtime jump references
    private float _cyoteTimer;
    private float _initialJumpVelocity;
    private float _jumpGravity;

    //dash
    private float _dashIntervalTimer;
    private float _dashRegenTimer;
    private int _currentDashCharges;
    private float _initialDashVelocity;
    private float _dashGravity;

    #endregion
    #region Getters/Setters
    //SerialisedField Setters
    public CharacterController Controller { get { return _characterController; } }
    public Transform Orientation { get { return _orientation; } }
    public Transform PlayerCameraPosition { get { return _playerCameraPosition; } }
    public Transform FaceDirection { get { return _faceDirection; } }
    public GameObject MainCamera { get { return _mainCamera; } }
    public GameObject AimCamera { get { return _aimCamera; } }
    public PlayerVariables Variables { get { return _playerVariables; } }

    //Script References
    public PlayerInputHandler Input { get { return _input; } }
    public PlayerBaseState CurrentMovementState { get { return _currentMovementState; } set { _currentMovementState = value; } }
    public PlayerCameraBaseState CurrentCameraState { get { return _currentCameraState; } set { _currentCameraState = value; } }
    public PlayerMotor PlayerMotor { get { return _playerMotor; } }

    //Jump
    public float InitialJumpVelocity { get { return _initialJumpVelocity; } }
    public float TimeLeftOnGround { get { return _cyoteTimer; } set { _cyoteTimer = value; } }
    public bool CyoteTrue { get { return _cyoteTimer < 0; } }
    public float JumpGravity { get { return _jumpGravity; } }

    //dash
    public float DashCooldownTimer { get { return _dashIntervalTimer; } set { _dashIntervalTimer = value; } }
    public int AvalableDashCharges { get { return _currentDashCharges; } }
    public bool CanDash { get { return _dashIntervalTimer <= 0f && _currentDashCharges > 0; } }
    public float InitialDashVelocity { get { return _initialDashVelocity; } }
    public float DashGravity { get { return _dashGravity; } }

    //Ground
    public bool IsOnGround { get { return Controller.isGrounded; } }
    #endregion

    //updatemethods
    private void Awake()
    {
        HideMouse();
        //Input and Motor
        if (_input == null)
            _input = GetComponent<PlayerInputHandler>();
        if (_playerMotor == null)
            _playerMotor = GetComponent<PlayerMotor>();
        //Movement SM
        _movementFactory = new PlayerStateFactory(this);
        _currentMovementState = _movementFactory.Grounded();
        _currentMovementState.EnterState();
        //Camera SM
        _cameraFactory = new PlayerCameraStateFactory(this);
        _currentCameraState = _cameraFactory.MainCamera();
        _currentCameraState.EnterState();
        //jump
        SetupJumpVariables();
        //dash
        SetupDashVariales();
        _currentDashCharges = _playerVariables.maxDashCharges;
    }

    private void Update()
    {
        CyoteTimer();
        DashIntervalTimer();
        DashRegenen();

        //State Updates
        _currentMovementState.UpdateStates();
        _playerMotor.UpdatePhysics();

        _currentCameraState?.UpdateStates();
    }

    //Helper Functions
    private void CyoteTimer()
    {
        if (_cyoteTimer > 0)
            _cyoteTimer -= Time.deltaTime;
    }
    private void SetupJumpVariables()
    {
        float _timeToApex = _playerVariables.maxJumpTime / 2;
        _jumpGravity = -2 * _playerVariables.maxJumpHeight / Mathf.Pow(_timeToApex, 2);
        _initialJumpVelocity = 2 * _playerVariables.maxJumpHeight / _timeToApex;
    }
    private void SetupDashVariales()
    {
        float _timeToApex = Variables.dashDuration / 2;
        _dashGravity = -2 * _playerVariables.samllDashJumpHeight / Mathf.Pow(_timeToApex, 2);
        _initialDashVelocity = 2 * Variables.samllDashJumpHeight / _timeToApex;
    }
    public void DashConsume()
    {
        _currentDashCharges--;
        _dashIntervalTimer = Variables.dashInterval;
        _dashRegenTimer = Variables.dashRegenTime;
    }
    public void DashIntervalTimer()
    {
        if (_dashIntervalTimer > 0f)
        {
            _dashIntervalTimer -= Time.deltaTime;
        }
    }
    public void DashRegenen()
    {
        if (_currentDashCharges >= Variables.maxDashCharges) return;

        if (_dashRegenTimer > 0)
        {
            _dashRegenTimer -= Time.deltaTime;
            return;
        }
        _currentDashCharges += 1;
        _currentDashCharges = Mathf.Min(_currentDashCharges, _playerVariables.maxDashCharges);
        _dashRegenTimer = _playerVariables.dashRegenTime;
    }
    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnHideMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}