using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    [SerializeField] private PlayerController _ctx;

    // Movement state
    private float _verticalVelocity;

    private Vector3 _movementVector;
    private float _gravity;
    private float _currentSpeed;
    private float _expectedSpeed;
    //dash variables
    private bool _isDashing;
    private float _dashTimer;

    //Getters and Setters
    //dash
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public float DashTime { get { return _dashTimer; } set { _dashTimer = value; } }
    public bool CanDash { get { return _dashTimer <= 0; } }
    //Upward force
    public float VerticalVelocity { get { return _verticalVelocity; } }
    //gravity
    public float Gravity { get { return _gravity; } }
    //movement
    public Vector3 FinalMoveVector { get { return CalculateFinalMoveVector(); } }

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
        Vector3 finalVelocity = CalculateFinalMoveVector();

        _ctx.Controller.Move(finalVelocity * Time.deltaTime);
    }

    #endregion
    #region Calculations
    /// Helper functions
    private void ApplyGravity()
    {
        if (_ctx.IsOnGround && _verticalVelocity < 0f)
        {
            // small downward force to stay grounded
            _verticalVelocity = -2f;
            return;
        }
        _verticalVelocity += Gravity * Time.deltaTime;
    }
    public Vector3 CalculateFinalMoveVector()
    {
        Vector3 horizontal;
        if (IsDashing)
            horizontal = _ctx.Orientation.forward * _currentSpeed;
        else
            horizontal = _movementVector * _currentSpeed;
        return horizontal + Vector3.up * _verticalVelocity;
    }

    //public API
    public void SetGroundMovementInput(Vector2 input)
    {
        // Debug.Log(_ctx?.GetType());
        Vector3 move = _ctx.Orientation.right * input.x + _ctx.Orientation.forward * input.y;
        move.y = 0f;
        _movementVector = move.normalized;
    }
    public void SetAirMovementInput(Vector2 input)
    {
        Vector3 move = _ctx.Orientation.right * input.x + _ctx.Orientation.forward * input.y;
        move.y = 0f;
        move = move.normalized;

        _movementVector = Vector3.MoveTowards(
            _movementVector,
            move,
            _ctx.Variables.airMoveSpeed * _ctx.Variables.airControll * Time.deltaTime
        );
        _movementVector = Vector3.ClampMagnitude(_movementVector, _ctx.Variables.maxAirSpeed);
    }

    //Dash
    private void DashReset()
    {
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f) { _isDashing = false; _currentSpeed = 0f; }
        }
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
    public void SetJumpVelocity(float upWardForce)
    {
        _verticalVelocity = upWardForce;
    }
    public void StartDash(float distance, float duration, Vector3 direction)
    {
        IsDashing = true;
        DashTime = duration;
        _currentSpeed = distance / duration;
        _movementVector = direction;
    }
    #endregion
}