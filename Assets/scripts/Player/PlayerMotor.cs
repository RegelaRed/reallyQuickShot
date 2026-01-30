using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    [SerializeField] private PlayerController _ctx;

    // Movement state
    private float _verticalVelocity;

    private Vector3 _movementVector;
    private Vector3 _dashDirection;
    private float _gravity;
    private float _speed;
    //dash variables
    private bool _isDashing;
    private float _dashTimer;

    //dash
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public float DashTime { get { return _dashTimer; } set { _dashTimer = value; } }
    public bool CanDash { get { return _dashTimer <= 0; } }

    //Getters and Setters
    public float VerticalVelocity { get { return _verticalVelocity; } }
    public float Gravity { get { return _gravity; } }

    public Vector3 FinalMoveVector { get { return CalculateFinalMoveVector(); } }

    #endregion
    #region Updates
    private void Awake()
    {
        if (_ctx == null)
            _ctx = GetComponent<PlayerController>();
    }

    public void UpdatePhysics()
    {
        //Dash Reset
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
            {
                _isDashing = false;
                _speed = 0f;
            }
        }


        ApplyGravity();

        Vector3 finalVelocity = CalculateFinalMoveVector();

        _ctx.Controller.Move(finalVelocity * Time.deltaTime);

        _dashDirection = Vector3.zero;
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
        // _verticalVecloity = Mathf.Max(_verticalVecloity, Gravity);
        _verticalVelocity += Gravity * Time.deltaTime;
    }
    public Vector3 CalculateFinalMoveVector()
    {
        // Vector3 finalMoveVector = MovementVector * _speed + (Vector3.up * _verticalVelocity) + ForwardInpulse;
        Vector3 horizontal;
        if (IsDashing)
            horizontal = _ctx.Orientation.forward * _speed;
        else
            horizontal = _movementVector * _speed;
        return horizontal + Vector3.up * _verticalVelocity;
    }

    //public API
    public void SetMovementInput(Vector2 input)
    {
        // Debug.Log(_ctx?.GetType());
        Vector3 move = _ctx.Orientation.right * input.x + _ctx.Orientation.forward * input.y;
        move.y = 0f;
        _movementVector = move.normalized;
    }
    public void SetGravity(float gravity)
    {
        _gravity = gravity;
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetJumpVelocity(float upWardForce)
    {
        _verticalVelocity = upWardForce;
    }
    public void StartDash(Vector3 direction, float distance, float duration)
    {
        IsDashing = true;
        DashTime = duration;
        _dashDirection = direction.normalized;
        _speed = distance / duration;
    }
    #endregion
}