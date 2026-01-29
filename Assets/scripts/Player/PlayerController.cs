using System.Collections;
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
    private PlayerInputHandler _input;
    private PlayerBaseState _currentState;
    private PlayerStateFactory _factory;
    private PlayerMotor _playerMotor;

    //runtime jump references
    private bool _isJumping;
    private float _timeLeftOnGround;
    private float _initialJumpVelocity;
    private float _jumpGravity;

    //dash variables
    private float _dashTimer;
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
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
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PlayerMotor PlayerMotor { get { return _playerMotor; } }

    //Jump
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
    public float InitialJumpVelocity { get { return _initialJumpVelocity; } }
    public float TimeLeftOnGround { get { return _timeLeftOnGround; } set { _timeLeftOnGround = value; } }
    public float JumpGravity { get { return _jumpGravity; } }

    //dash
    public float DashTime { set { _dashTimer = value; } }
    public bool CanDash { get { return _dashTimer <= 0; } }

    //Ground
    public bool IsOnGround { get { return Controller.isGrounded; } }
    #endregion

    //updatemethods
    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _playerMotor = GetComponent<PlayerMotor>();
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Grounded();
        _currentState.EnterState();

        SetupJumpVariables();
    }
    //tempdebug variables
    PlayerBaseState lastState;
    PlayerBaseState lastSubState;
    private void Update()
    {
        if (_dashTimer > 0f)
            _dashTimer -= Time.deltaTime;


        _currentState.UpdateStates();
        _playerMotor.UpdatePhysics();

        if (lastState != _currentState)
        {
            Debug.Log("Current Superstate" + _currentState?.GetType().Name);
            lastState = _currentState;
        }
        if (lastSubState != _currentState.CurrentSubState)
        {
            Debug.Log("Current SubState" + _currentState.CurrentSubState?.GetType().Name);
            lastSubState = _currentState.CurrentSubState;
        }
    }

    //Helper Functions
    private void SetupJumpVariables()
    {
        float _timeToApex = _playerVariables.maxJumpTime / 2;
        _jumpGravity = -2 * _playerVariables.maxJumpHeight / Mathf.Pow(_timeToApex, 2);
        _initialJumpVelocity = 2 * _playerVariables.maxJumpHeight / _timeToApex;
    }

}