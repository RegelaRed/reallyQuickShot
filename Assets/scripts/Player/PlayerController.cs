using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("References")]

    [SerializeField] private CharacterController _characterController;
    public CharacterController CController => _characterController;

    [SerializeField] private Transform _orientation;
    public Transform Orientation => _orientation;

    [SerializeField] private Transform _playerCameraPosition;
    public Transform PlayerCameraPosition => _playerCameraPosition;

    [SerializeField] private Transform _faceDirection;
    public Transform FaceDirection => _faceDirection;

    //Cinemachine Cameras
    [SerializeField] private GameObject _mainCamera;
    public GameObject MainCamera => _mainCamera;

    [SerializeField] private GameObject _aimCamera;
    public GameObject AimCamera => _aimCamera;

    [SerializeField] private PlayerVariables _playerVariables;
    public PlayerVariables PV => _playerVariables;

    private PlayerInputHandler _input;
    public PlayerInputHandler Input => _input;

    private PlayerStateFactory _state;
    private PlayerBaseState _currentState;

    //getters/setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    //static variables
    [SerializeField] private float _standHeight;
    public float StandHeight => _standHeight;
    #endregion

    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }
}