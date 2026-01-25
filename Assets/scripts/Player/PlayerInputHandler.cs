using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    //reference variables
    private PlayerInputActions _action;
    public Vector2 CurrentMovementInput { get; private set; }
    public Vector2 CurrentLookInput { get; private set; }
    public bool IsMovementPressed { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsDashPressed { get; private set; }

    private void Awake()
    {
        _action = new PlayerInputActions();

        _action.Player.Move.started += context => ReadMovementInput(context);
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
        CurrentMovementInput = context.ReadValue<Vector2>();
        IsMovementPressed = CurrentMovementInput.x != 0 || CurrentMovementInput.y != 0;
    }
    void OnLook(InputAction.CallbackContext context) { CurrentLookInput = context.ReadValue<Vector2>(); }
    void OnSprint(InputAction.CallbackContext context) { IsSprinting = context.ReadValueAsButton(); }
    void OnJump(InputAction.CallbackContext context) { IsJumpPressed = context.ReadValueAsButton(); }
    void OnDash(InputAction.CallbackContext context) { IsDashPressed = context.ReadValueAsButton(); }

    //playerInput requirements
    public void OnEnable() { _action.Player.Enable(); }
    public void OnDisable() { _action.Player.Disable(); }
    //Updates
    private void Update() { }
}
