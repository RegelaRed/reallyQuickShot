using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //reference variables
    private PlayerInputActions _action;
    private Vector2 _currentMovementInput;
    private Vector2 _currentLookInput;
    private bool _isMovementPressed;
    private bool _isSprintPressed;
    private bool _sprintToggle = false;
    private bool _isJumpPressedThisFrame;
    private bool _isJumpPressed;
    private bool _isDashPressedThisFrame;
    private bool _isDashPressed;


    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public Vector2 CurrentLookInput { get { return _currentLookInput; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsSprintPressed { get { return _isSprintPressed; } }
    public bool SprintToggle { get { return _sprintToggle; } }
    public bool IsJumpPressedThisFrame { get { return _isJumpPressedThisFrame; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsDashPressedThisFrame { get { return _isDashPressedThisFrame; } }
    public bool IsDashPressed { get { return _isDashPressedThisFrame; } }

    private void Awake()
    {
        _action = new PlayerInputActions();

        _action.Player.Move.canceled += context => ReadMovementInput(context);
        _action.Player.Move.performed += context => ReadMovementInput(context);

        _action.Player.Sprint.performed += context => OnSprint(context);
        _action.Player.Sprint.canceled += context => OnSprint(context);

        _action.Player.Jump.performed += context => OnJump(context);
        _action.Player.Jump.canceled += context => OnJump(context);

        _action.Player.Dash.performed += context => OnDash(context);
        _action.Player.Dash.canceled += context => OnDash(context);

        _action.Player.Look.performed += context => OnLook(context);
        _action.Player.Look.canceled += context => OnLook(context);
    }

    //calback references
    void ReadMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>().normalized;
        _isMovementPressed = CurrentMovementInput.x != 0 || CurrentMovementInput.y != 0;
    }
    void OnLook(InputAction.CallbackContext context) { _currentLookInput = context.ReadValue<Vector2>(); }
    void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            _sprintToggle = !_sprintToggle;
        _isSprintPressed = context.ReadValueAsButton();
    }
    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _isJumpPressedThisFrame = true;
        _isJumpPressed = context.ReadValueAsButton();
    }
    void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            _isDashPressedThisFrame = true;
        _isDashPressed = context.ReadValueAsButton();
    }

    //playerInput requirements
    public void OnEnable() { _action.Player.Enable(); }
    public void OnDisable() { _action.Player.Disable(); }
}
