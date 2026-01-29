using System.Collections;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
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
    private bool _requestJumpAgain;
    private float _initialJumpVelocity;
    private float _jumpGravity;

    //dash variables
    private bool _requestDashAgain = false;
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }


    //getters/setters
    public CharacterController Controller => _characterController;
    public Transform Orientation => _orientation;
    public Transform PlayerCameraPosition => _playerCameraPosition;
    public Transform FaceDirection => _faceDirection;
    public GameObject MainCamera => _mainCamera;
    public GameObject AimCamera => _aimCamera;
    public PlayerVariables Variables => _playerVariables;
    public PlayerInputHandler Input => _input;
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PlayerMotor PlayerMotor { get { return _playerMotor; } }

    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
    public bool RequestJumpAgain { get { return _requestJumpAgain; } set { _requestJumpAgain = value; } }
    public float InitialJumpVelocity => _initialJumpVelocity;
    public float JumpGravity => _jumpGravity;
    public bool RequestDashAgain { get { return _requestDashAgain; } set { _requestDashAgain = value; } }
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
        _currentState.UpdateStates();
        _playerMotor.UpdatePhysics();
        if (lastState != _currentState)
        {
            Debug.Log("Current Superstate" + _currentState?.GetType());
            lastState = _currentState;
        }
        if (lastSubState != _currentState.CurrentSubState)
        {
            Debug.Log("Current SubState" + _currentState.CurrentSubState);
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