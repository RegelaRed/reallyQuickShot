using UnityEngine;

/// <summary>
/// Dash ability state.
/// Provides rapid directional movement with zero gravity for the dash duration.
/// Exits when duration expires (and player has returned to ground if dash started airborne).
/// Consuming jump input during dash prevents chained jump-after-dash. <para/>
/// 
/// ASSUMES: <para/>
/// - Dash velocity has been pre-calculated into InitialDashHorizontalVelocity <para/>
/// 
/// GUARANTEES: <para/>
/// - Gravity is restored (JumpGravity) on exit <para/>
/// - Horizontal velocity is zeroed on exit <para/>
/// - Jump buffer is consumed to prevent accidental double-jumping <para/>
/// 
/// INTERRUPTS: <para/>
/// - Duration timer expiry → returns to Grounded or Falling based on ground state
/// </summary>
public class PlayerDashState : PlayerBaseState
{

    private bool _dashLeftGround;
    private float _dashDurationTimer;

    public PlayerDashState(PlayerStateFactory stateFactory, PlayerMotor playerMotor) :
     base(stateFactory, playerMotor)
    { }

    /// <summary>
    /// Disables gravity and initializes dash movement.
    /// </summary>
    public override void EnterState(PlayerContext context)
    {
        context.CurrentGravity = 0f;
        Motor.SetVerticalVelocity(0f);
        StartDash(context);
    }
    /// <summary>
    /// Restores gravity, stops horizontal movement, and consumes buffered jump. <para/>
    /// Jump consumption prevents chaining jump immediately after dash.
    /// </summary>
    public override void ExitState(PlayerContext context)
    {
        context.InputBuffer.ConsumeJump();
        context.CurrentSpeed = context.CurrentSpeed / 2;
        context.CurrentGravity = context.JumpGravity;
    }
    /// <summary>
    /// Updates dash direction and duration, checking for exit conditions.
    /// </summary>
    public override void UpdateState(PlayerContext context)
    {
        Motor.SetHorizontalDirectionVector(context.Input.DashDirection);
        UpdateDash(context);
    }
    /// <summary>
    /// Exits dash when duration expires.
    /// </summary>
    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        if (_dashDurationTimer > 0f) return null;
        return context.IsGrounded ? Factory.Grounded() : Factory.Falling();
    }
    public override void InitializeSubState(PlayerContext context) { }

    /// <summary>
    /// Initializes dash timer, cooldown, and velocity.
    /// Consumes one dash charge via cooldown system.
    /// </summary>
    public void StartDash(PlayerContext context)
    {
        _dashLeftGround = false;
        _dashDurationTimer = context.Variables.dashDuration;

        context.CanDashIntervalTimer = context.Variables.dashInterval;
        context.CurrentSpeed = context.InitialDashHorizontalVelocity;
        Motor.SetHorizontalDirectionVector(context.Input.DashDirection);
    }

    /// <summary>
    /// Decrements dash timer and tracks air status. <para/>
    /// Ensures dash only ends after player returns to ground (prevents instant completion if starting grounded).
    /// </summary>
    private void UpdateDash(PlayerContext context)
    {
        if (_dashDurationTimer > 0f)
        {
            _dashDurationTimer -= context.DeltaTime;
            return;
        }

        // Track first airborne frame after dash start
        if (!_dashLeftGround && !context.IsGrounded)
            _dashLeftGround = true;

        // Only truly end dash when player is grounded again AND has been airborne
        if (_dashLeftGround && context.IsGrounded)
            EndDash(context);

        if (_dashDurationTimer <= 0f)
            EndDash(context);

    }
    /// <summary>
    /// Resets dash timer and vertical velocity.
    /// </summary>
    private void EndDash(PlayerContext context)
    {
        _dashDurationTimer = 0f;
        Motor.VerticalVelocity = 0f;
    }
}