/// <summary>
/// Falling state for airborne movement when not jumping.
/// Applies gravity and allows air-strafing with horizontal movement input.
/// Exits immediately upon landing. <para/>
/// GUARANTEES: <para/>
/// - Gravity is active (JumpGravity) during fall <para/>
/// INTERRUPTS: <para/>
/// - Ground contact → returns to Grounded state
/// </summary>
public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    /// <summary>
    /// Ensures gravity is applied for the fall.
    /// </summary>
    public override void EnterState(PlayerContext context)
    {
        context.CurrentGravity = context.JumpGravity;
    }

    public override void ExitState(PlayerContext context) { }

    /// <summary>
    /// Applies air-strafing movement, allowing player to influence horizontal motion while falling.
    /// </summary>
    public override void UpdateState(PlayerContext context)
    {
        Motor.SetAirMovementDirection(context);
    }

    /// <summary>
    /// Transitions to Grounded when player touches the ground.
    /// </summary>
    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        if (context.IsGrounded)
            return Factory.Grounded();
        return null;
    }

    public override void InitializeSubState(PlayerContext context) { }
}