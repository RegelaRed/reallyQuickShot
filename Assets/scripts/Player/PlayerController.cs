using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCameraPosition;
    [SerializeField] private Transform _faceDirection;

    //Cinemachine Cameras
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _aimCamera;

    //scrpipt objects
    [SerializeField] private PlayerVariables _playerVariables;
    private PlayerInputActions _action;
    private PlayerStateFactory _state;
    private PlayerBaseState _currentState;

    //getters/setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    //static variables
    [SerializeField] private float _standHeight;


    //public access read-only
    public CharacterController CController => _characterController;
    public Transform Orientation => _orientation;
    public Transform PlayerCameraPosition => _playerCameraPosition;
    public Transform FaceDirection => _faceDirection;

    public GameObject MainCamera => _mainCamera;
    public GameObject AimCamera => _aimCamera;

    public PlayerVariables PV => _playerVariables;
    public PlayerInputActions Actions => _action;

    public float StandHeight => _standHeight;
    #endregion

    private void OEnable() { }
    private void ODisable() { }

    private void Awake()
    {

    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }
}