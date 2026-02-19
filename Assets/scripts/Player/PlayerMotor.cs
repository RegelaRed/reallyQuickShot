using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region References
    // Cached references
    [SerializeField] private CharacterController _controlelr;
    [SerializeField] private PlayerVariables _playerVariables;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _faceDirection;

    // Movement state
    private float _verticalFloat;
    private Vector3 _horizontalVelocity;
    private float _currentGravity;
    private float _currentSpeed;
    //dash variables
    private bool _isDashing;
    private float _dashTimer;
    private bool _dashLeftGround;


    //Getters and Setters

    //dash
    public bool IsDashing => _isDashing;
    public float DashTime => _dashTimer;
    public bool CanDash => _dashTimer <= 0;
    //Upward force
    public float VerticalVelocity => _verticalFloat;
    //gravity
    public float Gravity => _currentGravity;
    //movement
    public Vector3 FinalMoveVector => CalculateFinalMoveVector();
    public float CurrentSpeed => _currentSpeed;
    public Vector3 CurrentMovementVector => _horizontalVelocity;

    #endregion
    #region Updates
    private void Awake()
    {
        _currentSpeed = _playerVariables.walkSpeed;
    }

    public void UpdatePhysics(PlayerContext context)
    {
        ApplyGravity(context);
        DashStates(context);
        _controlelr.Move(CalculateFinalMoveVector() * Time.deltaTime);
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
        _verticalFloat += _currentGravity * Time.deltaTime;
    }
    public Vector3 CalculateFinalMoveVector()
    {
        return (_horizontalVelocity * _currentSpeed) + (Vector3.up * _verticalFloat);
    }

    #endregion
    #region public API
    /// <summary>
    /// sets the movement Vector for Moving on ground, offers full movement sensitivity
    /// </summary>
    /// <param name="input"> Player Context to be pased in </param>
    public void SetGroundMovementInput(PlayerContext context)
    {
        _horizontalVelocity = NormalizedInput(context.Input.Move);
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
    //Dash
    /// <summary>
    /// Starts the Dash Physics lifecycle
    /// Defined in PlayerMotor for simpler management of dash lifecycle  
    /// </summary>
    /// <param name="distance"> Distance the Dash Covers, used for Calculating Speed </param>
    /// <param name="duration"> Duration of Dash, Used for Calculating Speed and Dash State Lifetime </param>
    /// <param name="direction"> Direction of Dash </param>
    public void StartDash(float distance, float duration, Vector3 direction)
    {
        _isDashing = true;
        _dashLeftGround = false;

        _horizontalVelocity = Vector3.zero;
        _dashTimer = duration;
        _currentSpeed = distance / duration;
        _horizontalVelocity = direction.normalized;
    }
    /// <summary>
    /// Checks to reset dash related Variables and early Dash Exit
    /// </summary>
    /// <param name="context"> Player Context for GroundChecks </param>
    private void DashStates(PlayerContext context)
    {
        if (!_isDashing)
            return;

        if (!context.IsGrounded)
            _dashLeftGround = true;

        if (_dashLeftGround && context.IsGrounded)
            EndDash();

        if (_dashTimer > 0f)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
                EndDash();
        }
    }
    /// <summary>
    /// Reset dash related Varibles
    /// </summary>
    private void EndDash()
    {
        _isDashing = false;
        _currentSpeed = _currentSpeed / 4;
        _dashTimer = 0f;
        // _horizontalVelocity = _horizontalVelocity / 4;
    }
    //set variables
    /// <summary>
    /// Set Player Gravity
    /// </summary>
    /// <param name="gravity"> float value for gravity </param>
    public void SetGravity(float gravity)
    {
        if (_currentGravity != gravity)
            _currentGravity = gravity;
    }
    /// <summary>
    /// Set Player Speed
    /// </summary>
    /// <param name="speed"> float Speed </param>
    public void SetSpeed(float speed)
    {
        if (_currentSpeed != speed)
            _currentSpeed = speed;
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