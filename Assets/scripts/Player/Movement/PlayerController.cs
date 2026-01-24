using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _faceDirection;

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _aimCamera;
    // [SerializeField] private GameObject _aimReticle;

    [SerializeField] private PlayerVariables _playerVariables;
    [SerializeField] private float _standHeight = 2f;
    [SerializeField] private bool _debug = false;

    // Public access (read-only)
    public Rigidbody Rb => _rigidBody;
    public Transform Orientation => _orientation;
    public Transform PlayerCamera => _playerCamera;
    public Transform FaceDirection => _faceDirection;

    public GameObject MainCamera => _mainCamera;
    public GameObject AimCamera => _aimCamera;

    public PlayerVariables PlayerVariables => _playerVariables;
    public float StandHeight => _standHeight;
    public bool Debug => _debug;

    // Modules
    public PlayerInput Input { get; private set; }
    public PlayerCamera Camera { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerGrounded Ground { get; private set; }
    public PlayerDash Dash { get; private set; }
    public PlayerJump Jump { get; private set; }
    public StateMachine State { get; private set; }
    #endregion
    #region Updates
    private void Awake()
    {
        _rigidBody.freezeRotation = true;

        Input = new PlayerInput(this);
        Movement = new PlayerMovement(this);
        Ground = new PlayerGrounded(this);
        Dash = new PlayerDash(this);
        Jump = new PlayerJump(this);
        State = new StateMachine(this);


        Camera = new PlayerCamera(this);
        Camera.LockPlayerCursor();
    }
    private void Update()
    {
        Input.Tick();
        Camera.Tick();
        Ground.Tick();
        State.Tick();
        Dash.Tick();
        Jump.Tick();
    }
    private void FixedUpdate()
    {
        Movement.Tick();
    }
    #endregion
}
