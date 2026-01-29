using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    private PlayerController _ctx;

    // Movement state
    private float _verticalVecloity;

    private Vector3 _movementVector;
    private Vector3 _forwardInpulse;
    private float _gravity;
    private float _speed;

    //Getters and Setters
    public float VerticalVelocity { get { return _verticalVecloity; } }
    public Vector3 MovementVector { get { return _movementVector; } }
    public Vector3 ForwardInpulse { get { return _forwardInpulse; } }
    public float Speed { get { return _speed; } }
    public float Gravity { get { return _gravity; } set { _gravity = value; } }

    //dash references
    private Vector3 _dashImpulse;

    //private bool _isGrounded => _ctx.Controller.isGrounded;

    #endregion
    //
    //update functions
    private void Awake()
    {
        _ctx = GetComponent<PlayerController>();
    }
    public void UpdatePhysics()
    {
        ApplyGravity();

        Vector3 finalVelocity = FinalMovevector();

        _ctx.Controller.Move(finalVelocity * Time.deltaTime);

        _dashImpulse = Vector3.zero;
    }

    /// Helper functions
    private Vector3 FinalMovevector()
    {
        Vector3 finalMoveVector = MovementVector * _speed + (Vector3.up * _verticalVecloity) + ForwardInpulse;
        return finalMoveVector;
    }

    //public API
    public void SetMovementInput(Vector2 input)
    {
        _movementVector.x = input.x;
        _movementVector.z = input.y;
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetJumpVelocity(float upWardForce)
    {
        _verticalVecloity = upWardForce;
    }
    public void SetInpulse(Vector3 inpulse)
    {
        _forwardInpulse = inpulse;
    }

    private void ApplyGravity()
    {
        if (_ctx.IsOnGround && _verticalVecloity < 0f)
        {
            // small downward force to stay grounded
            _verticalVecloity = -2f;
            return;
        }
        _verticalVecloity = Mathf.Clamp(_verticalVecloity, _verticalVecloity, Gravity);
        _verticalVecloity += Gravity * Time.deltaTime;
    }
}

/// methods needed
/// set gravity
/// set speed
/// set input
/// set vertical velocity(Jump Force)
/// set Inpulse(Dash Force) 

// public void SetGravity(float gravity)
// {
//     _gravity = gravity;
// }
// public void SetSpeed(float speed)
// {
//     _speed = speed;
// }
// public void SetMoveInput(Vector2 moveInput)
// {
//     _movementVector.x = moveInput.x;
//     _movementVector.z = moveInput.y;
// }
// public void SetUpwardForce(float upForce)
// {
//     _upWardForce = upForce;
// }


/// <summary> Set Movement direction </summary>
/// <param name="velocity"></param>
// public void SetHorizontalVelocity(Vector2 velocity, float speed)
// {
//     Debug.Log(velocity);
//     _horizontalVelocity = new Vector3(velocity.x, 0, velocity.y) * speed;
// }

// /// <summary>Set Jump Force</summary>
// /// <param name="jumpVelocity"></param>
// public void ApplyJumpForce(float jumpVelocity, float gravity)
// {
//     _verticalVelocity = jumpVelocity;
//     _gravity = gravity;
// }

// /// <summary>Set Dash Force</summary>
// /// <param name="impulse"></param>
// public void ApplyImpulse(Vector3 impulse)
// {
//     _dashImpulse = impulse;
// }
