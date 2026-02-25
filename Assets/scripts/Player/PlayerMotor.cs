using System.Net.Http.Headers;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _orientation;

    // Movement state
    private float _verticalFloat;
    private Vector3 _horizontalVelocity;
    private float _currentGravity;
    private float _currentSpeed;

    private Vector3 _finalMoveVector;
    //Getters and Setters

    //Upward force
    public float VerticalVelocity { get => _verticalFloat; set => _verticalFloat = value; }
    //gravity
    public float Gravity => _currentGravity;
    //movement
    public Vector3 FinalMoveVector => _finalMoveVector;
    public float CurrentSpeed => _currentSpeed;
    public Vector3 CurrentMovementVector => _horizontalVelocity;
    public Vector3 HorizontalVelocityVector { get => _horizontalVelocity; set => _horizontalVelocity = value; }


    #endregion
    #region Updates
    private void Awake()
    {
        _currentSpeed = 1f;
        _controller ??= GetComponent<CharacterController>();
    }

    public void UpdatePhysics(PlayerContext context)
    {
        ApplyGravity(context);
        _finalMoveVector = CalculateFinalMoveVector(context);
        _controller.Move(_finalMoveVector * Time.deltaTime);
    }

    #endregion
    #region Calculations
    private void ApplyGravity(PlayerContext context)
    {

        if (context.IsGrounded && _verticalFloat < 0f)
        {
            // small downward force to stay grounded
            _verticalFloat = -2f;
            return;
        }
        _verticalFloat += context.CurrentGravity * Time.deltaTime;
    }

    public Vector3 CalculateFinalMoveVector(PlayerContext context)
    {
        return (_horizontalVelocity * context.CurrentSpeed) + (Vector3.up * _verticalFloat);
    }

    #endregion
    #region public API

    public void SetHorizontalVelocity(Vector3 direction)
    {
        _horizontalVelocity = direction.normalized;
    }
    public void SetVerticalVelocity(float force)
    {
        _verticalFloat = force;
    }
    /// <summary>
    /// sets the movement Vector for Moving on ground<para/>
    /// also smooths speed transitions
    /// </summary>
    /// <param name="input"> Player Context to be pased in </param>
    public void SetGroundMovementInput(PlayerContext context)
    {
        Vector3 move = NormalizedInput(context.Input.Move);
        move.y = 0f;
        if (move.magnitude > 0.1f)
        {
            _horizontalVelocity = move;
        }
        else
        {
            _horizontalVelocity = Vector3.MoveTowards(
                _horizontalVelocity,
                Vector3.zero,
                context.Variables.groundDeceleration * context.DeltaTime
            );
        }
    }
    /// <summary>
    /// sets the movement Vector for Moving while in air, reduced movement sensitivity 
    /// </summary>
    /// <param name="context"> Player Context to be pased in </param>
    public void SetAirMovementInput(PlayerContext context)
    {
        Vector3 move = NormalizedInput(context.Input.Move);

        _horizontalVelocity = Vector3.MoveTowards(
            _horizontalVelocity, move,
            context.Variables.airMoveSpeed * context.Variables.airControl * Time.deltaTime
        );
        _horizontalVelocity = Vector3.ClampMagnitude(_horizontalVelocity, context.Variables.maxAirSpeed);
    }

    /// <summary>
    /// Normalizes and converts Vector2 Input to Vector3 
    /// </summary>
    /// <param name="input"> Vector2 of Input axis </param>
    /// <returns> Vector3 normalized direction</returns>
    private Vector3 NormalizedInput(Vector2 input)
    {
        Vector3 move = _orientation.right * input.x + _orientation.forward * input.y;
        move.y = 0f;

        return move.normalized;
    }
    /// <summary>
    /// Set Upward force for Jump
    /// </summary>
    /// <param name="upWardForce"> float Upward Force</param>
    public void SetUpwardVelocity(float upWardForce)
    {
        _verticalFloat = upWardForce;
    }
    #endregion
}