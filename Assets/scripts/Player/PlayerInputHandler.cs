using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Private References
    //reference variables
    private PlayerInputActions _action;
    //walk
    private Vector2 _currentMovementInput;
    private bool _isMovementPressed;
    //Look
    private Vector2 _currentLookInput;
    //sprint
    private bool _isSprintPressed;
    private bool _sprintToggle = false;
    //jump
    private float _jumpBufferTimer;
    private bool _isJumpPressedThisFrame;
    private bool _isJumpPressed;
    //dash
    private float _dashBufferTimer;
    private bool _isDashPressedThisFrame;
    private bool _isDashPressed;
    //Camera 
    private bool _aimToggle = false;
    //Attack
    private bool _attackHeld;
    private bool _attackPressed;
    private bool _reloadPressed;

    //weapon switching
    private bool _ammoPrevious;
    private bool _ammoNext;
    private bool _switchWeaponPressed;

    #endregion
    #region Getters/Setters
    //Walk
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    //Sprint
    public bool IsSprintPressed { get { return _isSprintPressed; } }
    public bool SprintToggle { get { return _sprintToggle; } }
    //Jump
    public float JumpBufferTimer { get { return _jumpBufferTimer; } set { _jumpBufferTimer = value; } }
    public bool JumpBufferActive { get { return _jumpBufferTimer > 0f; } }
    public bool IsJumpPressedThisFrame { get { return _isJumpPressedThisFrame; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    //Dash
    public float DashBufferTimer { get { return _dashBufferTimer; } set { _dashBufferTimer = value; } }
    public bool DashBufferActive { get { return _dashBufferTimer > 0f; } }
    public bool IsDashPressedThisFrame { get { return _isDashPressedThisFrame; } }
    public bool IsDashPressed { get { return _isDashPressed; } }
    //Camera
    public Vector2 CurrentLookInput { get { return _currentLookInput; } }
    //Attack
    public bool AttackHeld { get { return _attackHeld; } }
    public bool AttackPressed { get { return _attackPressed; } }
    public bool ReloadPressed { get { return _reloadPressed; } }
    //Attack -> Ranged
    public bool IsAiming { get { return _attackHeld || _aimToggle; } }

    //weapon switching
    public bool AmmoPrevious { get { return _ammoPrevious; } }
    public bool AmmoNext { get { return _ammoNext; } }

    public bool SwitchWeaponPressed { get { return _switchWeaponPressed; } }

    #endregion
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

        _action.Player.Attack.performed += context => OnAttack(context);
        _action.Player.Attack.canceled += context => OnAttack(context);

        _action.Player.Reload.performed += context => OnReolad(context);
        _action.Player.Reload.canceled += context => OnReolad(context);

        _action.Player.AimMode.performed += context => OnAimEnabled(context);
        _action.Player.AimMode.canceled += context => OnAimEnabled(context);

        _action.Player.Previous.performed += context => OnPrevious(context);
        _action.Player.Previous.canceled += context => OnPrevious(context);

        _action.Player.Next.performed += context => OnNext(context);
        _action.Player.Next.canceled += context => OnNext(context);

        _action.Player.SwitchWeapon.performed += context => OnSwitchWeapon(context);
        _action.Player.SwitchWeapon.canceled += context => OnSwitchWeapon(context);
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            _switchWeaponPressed = true;
    }

    private void OnReolad(InputAction.CallbackContext context)
    {
        _reloadPressed = context.ReadValueAsButton();
    }
    #region calback references
    void ReadMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>().normalized;
        _isMovementPressed = CurrentMovementInput.x != 0 || CurrentMovementInput.y != 0;
    }
    void OnLook(InputAction.CallbackContext context)
    {
        _currentLookInput = context.ReadValue<Vector2>();
    }
    void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            _sprintToggle = !_sprintToggle;
        _isSprintPressed = context.ReadValueAsButton();
    }
    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isJumpPressedThisFrame = true;
            _jumpBufferTimer = 0.15f;
        }
        _isJumpPressed = context.ReadValueAsButton();
    }
    void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isDashPressedThisFrame = true;
            _dashBufferTimer = 0.15f;
        }
        _isDashPressed = context.ReadValueAsButton();
    }
    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            _attackPressed = true;
        _attackHeld = context.ReadValueAsButton();
    }
    void OnAimEnabled(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _aimToggle = !_aimToggle;
        }
    }
    private void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
            _ammoPrevious = true;
    }
    private void OnNext(InputAction.CallbackContext context)
    {
        if (context.performed)
            _ammoNext = true;
    }
    #endregion
    //playerInput requirements
    public void OnEnable() { _action.Player.Enable(); }
    public void OnDisable() { _action.Player.Disable(); }

    private void LateUpdate()
    {
        _isJumpPressedThisFrame = false;
        _isDashPressedThisFrame = false;
        _attackPressed = false;
        _ammoNext = false;
        _ammoPrevious = false;
        _switchWeaponPressed = false;
        //delays
        float T = Time.deltaTime;
        if (_jumpBufferTimer > 0f) _jumpBufferTimer -= T;
        if (_dashBufferTimer > 0f) _dashBufferTimer -= T;
    }
}