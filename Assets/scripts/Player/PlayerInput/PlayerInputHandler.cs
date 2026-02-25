using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Private References
    // ------------ reference variables ------------
    private PlayerInputActions _action;
    // ------------ walk ------------ 
    private Vector2 _currentMovementInput;
    private bool _isMovementPressed;
    // ------------ Look ------------ 
    private Vector2 _currentLookInput;
    // ------------ sprint ------------ 
    private bool _isSprintPressed;
    private bool _sprintToggle = false;
    // ------------ jump ------------ 
    private float _jumpBufferTimer;
    private bool _isJumpPressedThisFrame;
    private bool _isJumpPressed;
    // ------------ dash ------------ 
    private float _dashBufferTimer;
    private bool _isDashPressedThisFrame;
    private bool _isDashPressed;
    // ------------ Camera ------------  
    private bool _aimToggle = false;
    // ------------ Attack ------------ 
    private bool _attackHeld;
    private bool _attackPressed;
    private bool _reloadPressed;
    // ------------ Weapon switching ------------ 
    private bool _weaponPrevious;
    private bool _weaponNext;
    private bool _switchAmmoPressed;

    #endregion
    #region Getters/Setters
    // ------------ Walk ------------ 
    public Vector2 CurrentMovementInput => _currentMovementInput;
    public bool IsMovementPressed => _isMovementPressed;
    // ------------ Sprint ------------ 
    public bool IsSprintPressed => _isSprintPressed;
    public bool SprintToggle => _sprintToggle;
    // ------------ Jump ------------ 
    public float JumpBufferTimer { get => _jumpBufferTimer; set => _jumpBufferTimer = value; }
    public bool JumpBufferActive => _jumpBufferTimer > 0f;
    public bool IsJumpPressedThisFrame => _isJumpPressedThisFrame;
    public bool IsJumpPressed => _isJumpPressed;
    // ------------ Dash ------------ 
    public float DashBufferTimer { get => _dashBufferTimer; set => _dashBufferTimer = value; }
    public bool DashBufferActive => _dashBufferTimer > 0f;
    public bool IsDashPressedThisFrame => _isDashPressedThisFrame;
    public bool IsDashPressed => _isDashPressed;
    // ------------ Camera ------------ 
    public Vector2 CurrentLookInput => _currentLookInput;
    // ------------ Attack ------------ 
    public bool AttackHeld => _attackHeld;
    public bool AttackPressed => _attackPressed;
    public bool ReloadPressed => _reloadPressed;
    // ------------ Ranged ------------ 
    public bool IsAiming => _attackHeld || _aimToggle;
    public bool AimToggle => _aimToggle;

    // ------------ Weapon switching ------------ 
    public bool WeaponPrevious => _weaponPrevious;
    public bool WeaponNext => _weaponNext;
    public bool SwitchAmmoPressed => _switchAmmoPressed;

    #endregion
    //------------------------------------------------
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

        _action.Player.WeaponPrevious.performed += context => OnWeaponPrevious(context);
        _action.Player.WeaponPrevious.canceled += context => OnWeaponPrevious(context);

        _action.Player.WeaponNext.performed += context => OnWeaponNext(context);
        _action.Player.WeaponNext.canceled += context => OnWeaponNext(context);

        _action.Player.SwitchAmmo.performed += context => OnSwitchAmmo(context);
        _action.Player.SwitchAmmo.canceled += context => OnSwitchAmmo(context);
    }
    private void LateUpdate()
    {
        _isJumpPressedThisFrame = false;
        _isDashPressedThisFrame = false;
        _attackPressed = false;
        _weaponNext = false;
        _weaponPrevious = false;
        _switchAmmoPressed = false;
        //delays
        float deltaTime = Time.deltaTime;
        if (_jumpBufferTimer > 0f) _jumpBufferTimer -= deltaTime;
        if (_dashBufferTimer > 0f) _dashBufferTimer -= deltaTime;
    }
    #region calback references
    // ------------ Movement ------------ 
    void ReadMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>().normalized;
        _isMovementPressed = CurrentMovementInput.x != 0 || CurrentMovementInput.y != 0;
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

    // ------------ Camera ------------
    void OnLook(InputAction.CallbackContext context)
    {
        _currentLookInput = context.ReadValue<Vector2>();
    }
    void OnAimEnabled(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _aimToggle = !_aimToggle;
        }
    }

    //------------ Attack ------------
    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            _attackPressed = true;
        _attackHeld = context.ReadValueAsButton();
    }
    private void OnReolad(InputAction.CallbackContext context)
    {
        _reloadPressed = context.ReadValueAsButton();
    }

    // ------------ Ammo/Weapon Switch ------------
    private void OnSwitchAmmo(InputAction.CallbackContext context)
    {
        if (context.performed)
            _switchAmmoPressed = true;
    }
    private void OnWeaponPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
            _weaponPrevious = true;
    }
    private void OnWeaponNext(InputAction.CallbackContext context)
    {
        if (context.performed)
            _weaponNext = true;
    }
    #endregion
    //-------- New Player Input System requirements ------------
    public void OnEnable() { _action.Player.Enable(); }
    public void OnDisable() { _action.Player.Disable(); }
}