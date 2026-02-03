using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    [SerializeField] private PlayerController _ctx;

    // Movement state
    private float _verticalFloat;
    private Vector3 _horizontalVelocity;
    private float _gravity;
    private float _currentSpeed;
    //dash variables
    private bool _isDashing;
    private float _dashTimer;

    //Getters and Setters

    //dash
    public bool IsDashing { get { return _isDashing; } }
    public float DashTime { get { return _dashTimer; } }
    public bool CanDash { get { return _dashTimer <= 0; } }
    //Upward force
    public float VerticalVelocity { get { return _verticalFloat; } }
    //gravity
    public float Gravity { get { return _gravity; } }
    //movement
    public Vector3 FinalMoveVector { get { return CalculateFinalMoveVector(); } }
    public float CurrentSpeed { get { return _currentSpeed; } }
    public Vector3 CurrentMovementVector { get { return _horizontalVelocity; } }



    #endregion
    #region Updates
    private void Awake()
    {
        if (_ctx == null)
            _ctx = GetComponent<PlayerController>();
        _currentSpeed = _ctx.Variables.walkSpeed;
    }

    public void UpdatePhysics()
    {
        DashReset();
        ApplyGravity();
        _ctx.Controller.Move(CalculateFinalMoveVector() * Time.deltaTime);
    }

    #endregion
    #region Calculations
    private void ApplyGravity()
    {
        if (_ctx.IsOnGround && _verticalFloat < 0f)
        {
            // small downward force to stay grounded
            _verticalFloat = -2f;
            return;
        }
        _verticalFloat += _gravity * Time.deltaTime;
    }
    public Vector3 CalculateFinalMoveVector()
    {
        return (_horizontalVelocity * _currentSpeed) + (Vector3.up * _verticalFloat);
    }

    #endregion
    #region public API
    public void SetGroundMovementInput(Vector2 input)
    {
        _horizontalVelocity = NormalizedInput(input);
    }
    public void SetAirMovementInput(Vector2 input)
    {
        Vector3 move = NormalizedInput(input);

        _horizontalVelocity = Vector3.MoveTowards(
            _horizontalVelocity, move,
            _ctx.Variables.airMoveSpeed * _ctx.Variables.airControl * Time.deltaTime
        );
        _horizontalVelocity = Vector3.ClampMagnitude(_horizontalVelocity, _ctx.Variables.maxAirSpeed);
    }
    private Vector3 NormalizedInput(Vector2 input)
    {
        Vector3 move = _ctx.Orientation.right * input.x + _ctx.Orientation.forward * input.y;
        move.y = 0f;

        return move.normalized;
    }
    //Dash
    public void StartDash(float distance, float duration, Vector3 direction)
    {
        _horizontalVelocity = Vector3.zero;
        _isDashing = true;
        _dashTimer = duration;
        _currentSpeed = distance / duration;
        _horizontalVelocity = direction.normalized;
    }
    //set variables
    public void SetGravity(float gravity)
    {
        _gravity = gravity;
    }
    public void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }
    public void SetUpwardVelocity(float upWardForce)
    {
        _verticalFloat = upWardForce;
    }
    public void CheckAimModeSpeed(float baseSpeed)
    {
        if (_ctx.Input.AttackHeld)
            _currentSpeed = _ctx.Variables.aimModeSpeed;
        else
            _currentSpeed = baseSpeed;
    }
    private void DashReset()
    {
        if (!_isDashing)
            return;

        if (_dashTimer > 0f)
        {
            _dashTimer -= Time.deltaTime;
            return;
        }
        _isDashing = false;
        _currentSpeed = 0f;
    }
    #endregion
}