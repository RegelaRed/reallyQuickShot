using UnityEngine;

/// <summary>
/// Handles all player movement physics and CharacterController motion.
/// <para/>
/// Responsibilities:
/// • Applies gravity
/// • Calculates final movement vector
/// • Moves CharacterController
/// • Stores current movement velocities
/// <para/>
/// Movement logic decisions are handled by the movement state machine.
/// PlayerMotor only executes movement physics.
/// </summary>
public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached scene references and runtime movement data
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _orientation;

    // Movement state
    /// <summary>
    /// Vertical velocity affected by gravity and jumping.
    /// Units: meters per second.
    /// </summary>
    private float _verticalVelocity;
    /// <summary>
    /// Horizontal movement direction vector.
    /// Usually normalized.
    /// </summary>
    private Vector3 _moveDirection;

    /// <summary>
    /// Final movement vector passed to CharacterController.Move().
    /// Includes both horizontal and vertical motion.
    /// Units: meters per frame.
    /// </summary>
    private Vector3 _finalMoveVector;
    //Getters and Setters

    //Upward force
    public float VerticalVelocity { get => _verticalVelocity; set => _verticalVelocity = value; }
    //movement
    public Vector3 FinalMoveVector => _finalMoveVector;
    public Vector3 CurrentMoveDirection => _moveDirection;

    #endregion
    #region Updates
    /// <summary>
    /// Initializes cached references and default movement values.
    /// </summary>
    private void Awake()
    {
        _controller ??= GetComponent<CharacterController>();
    }

    /// <summary>
    /// Updates movement physics for this frame.
    /// <para/>
    /// Steps:
    /// 1. Apply gravity to vertical velocity
    /// 2. Calculate final movement vector
    /// 3. Move CharacterController
    /// </summary>
    public void TickPhysics(PlayerContext context)
    {
        ApplyGravity(context);
        _finalMoveVector = CalculateMoveVector(context);
        _controller.Move(_finalMoveVector);
    }

    #endregion
    #region Calculations
    /// <summary>
    /// Applies gravity to vertical velocity.
    /// <para/>
    /// When grounded and moving downward, a small negative force
    /// is applied to keep the CharacterController grounded.
    /// </summary>
    private void ApplyGravity(PlayerContext context)
    {

        if (context.IsGrounded && _verticalVelocity < 0f)
        {
            // small downward force to stay grounded
            _verticalVelocity = -2f;
            return;
        }
        _verticalVelocity += context.CurrentGravity * Time.deltaTime;
    }

    /// <summary>
    /// Combines horizontal and vertical velocities into the final
    /// movement vector applied this frame.
    /// <para/>
    /// Horizontal movement uses CurrentSpeed scaling.
    /// Vertical movement uses accumulated vertical velocity.
    /// </summary>
    /// <returns>Movement vector in meters per frame</returns>
    public Vector3 CalculateMoveVector(PlayerContext context)
    {
        Vector3 horizontalDir = _moveDirection * context.CurrentSpeed * context.DeltaTime;
        Vector3 verticalVelocity = Vector3.up * _verticalVelocity * context.DeltaTime;
        return horizontalDir + verticalVelocity;
    }

    #endregion
    #region public API

    /// <summary>
    /// Sets horizontal movement direction.
    /// Direction is normalized before storing.
    /// </summary>
    public void SetHorizontalDirectionVector(Vector3 direction) => _moveDirection = direction.normalized;

    /// <summary>
    /// Sets vertical velocity directly.
    /// Typically used for jumping and dash impulses.
    /// </summary>
    /// <param name="verticalVelocity">Vertical velocity in meters per second</param>
    public void SetVerticalVelocity(float verticalVelocity) => _verticalVelocity = verticalVelocity;

    /// <summary>
    /// Applies ground movement input.
    /// <para/>
    /// • Converts input into movement direction
    /// • Applies deceleration when no input is present
    /// • Produces smooth stopping behavior
    /// </summary>
    /// <param name="context">Current player context</param>
    public void SetGroundMovementDirection(PlayerContext context)
    {
        Vector3 move = NormalizedDirectionVector(context.Input.Move);
        move.y = 0f;
        if (move.magnitude > 0.1f)
        {
            _moveDirection = move;
        }
        else
        {
            _moveDirection = Vector3.MoveTowards(
                _moveDirection,
                Vector3.zero,
                context.Variables.groundDeceleration * context.DeltaTime
            );
        }
    }

    /// <summary>
    /// Applies air movement input.
    /// <para/>
    /// Movement direction is gradually adjusted toward input direction
    /// using air control acceleration.
    /// <para/>
    /// Final velocity is clamped to maximum air speed.
    /// </summary>
    /// <param name="context">Current player context</param>
    public void SetAirMovementDirection(PlayerContext context)
    {
        Vector3 move = NormalizedDirectionVector(context.Input.Move);

        _moveDirection = Vector3.MoveTowards(
            _moveDirection, move,
            context.Variables.airMoveSpeed * context.Variables.airControl * Time.deltaTime
        );
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, context.Variables.maxAirSpeed);
    }

    /// <summary>
    /// Converts 2D input axes into a world-space movement direction
    /// relative to player orientation.
    /// <para/>
    /// Result is normalized and constrained to the horizontal plane.
    /// </summary>
    /// <param name="input">Raw input axis values</param>
    /// <returns>Normalized world-space movement direction</returns>
    private Vector3 NormalizedDirectionVector(Vector2 input)
    {
        Vector3 move = _orientation.right * input.x + _orientation.forward * input.y;
        move.y = 0f;

        return move.normalized;
    }
    #endregion
}