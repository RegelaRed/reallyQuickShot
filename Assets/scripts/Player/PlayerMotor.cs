using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    private PlayerController _ctx;
    private PlayerVariables _variables;

    // Movement state
    private Vector3 _horizontalVelocity;
    private float _verticalVelocity;
    private float _gravity;

    //dash references
    private Vector3 _dashImpulse;

    //private bool _isGrounded => _ctx.Controller.isGrounded;

    #endregion
    //
    //update functions
    private void Awake()
    {
        _ctx = GetComponent<PlayerController>();
        _variables = _ctx.Variables;
    }
    public void UpdatePhysics()
    {
        ApplyGravity();

        Vector3 finalVelocity = _horizontalVelocity + (Vector3.up * _verticalVelocity) + _dashImpulse;
        _ctx.Controller.Move(finalVelocity * Time.deltaTime);

        _dashImpulse = Vector3.zero;
    }

    /// Helper functions
    private void ApplyGravity()
    {
        if (_ctx.Controller.isGrounded && _verticalVelocity < 0f)
        {
            // small downward force to stay grounded
            _verticalVelocity = -2f;
            return;
        }
        _verticalVelocity += _gravity * Time.deltaTime;
    }

    //public API

    public void SetGravity(float gravity)
    {
        _gravity = gravity;
    }
    /// <summary> Set Movement direction </summary>
    /// <param name="velocity"></param>
    public void SetHorizontalVelocity(Vector2 velocity, float speed)
    {
        Debug.Log(velocity);
        _horizontalVelocity = new Vector3(velocity.x, 0, velocity.y) * speed;
    }

    /// <summary>Set Jump Force</summary>
    /// <param name="jumpVelocity"></param>
    public void ApplyJumpForce(float jumpVelocity, float gravity)
    {
        _verticalVelocity = jumpVelocity;
        _gravity = gravity;
    }

    /// <summary>Set Dash Force</summary>
    /// <param name="impulse"></param>
    public void ApplyImpulse(Vector3 impulse)
    {
        _dashImpulse = impulse;
    }
}