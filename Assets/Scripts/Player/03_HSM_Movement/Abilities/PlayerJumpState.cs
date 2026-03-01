/// <summary>
/// Jump ability state.
/// Maintains active jump state and applies upward velocity boost while jump button is held.
/// Exit occurs when jump duration expires or player touches ground. <para/>
/// ASSUMES: <para/>
/// - Vertical velocity has been pre-calculated from player power/gravity <para/>
/// INTERRUPTS: <para/>
/// - Dash (transitions to Dash if conditions met) <para/>
/// - Grounded (returns to ground state) <para/>
/// - Falling (transitions to Falling if jump duration expires while airborne)
/// </summary>
public class PlayerJumpState : PlayerBaseState
{

    float _jumpTimer;
    public PlayerJumpState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    public override void EnterState(PlayerContext context)
    {
        context.IsJumping = true;

        _jumpTimer = context.Variables.maxJumpDuration;
        context.CurrentSpeed = context.Input.SprintToggle ? context.Variables.sprintSpeed : context.Variables.walkSpeed;
        Motor.SetVerticalVelocity(context.InitialJumpVerticalVelocity);
    }
    public override void ExitState(PlayerContext context)
    {
        context.IsJumping = false;
        context.CurrentSpeed =
            context.Input.SprintToggle ?
            context.Variables.sprintSpeed : context.Variables.walkSpeed;
    }
    public override void UpdateState(PlayerContext context)
    {
        Motor.SetAirMovementDirection(context);
        if (_jumpTimer >= 0f) _jumpTimer -= context.DeltaTime;
    }

    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        if (DashRules.CanDash(context) && context.Input.DashPressed)
        {
            DashRules.Consume(context);
            return Factory.Dash();
        }

        if (_jumpTimer <= 0f || context.IsGrounded)
        {
            context.InputBuffer.ConsumeJump();
            return context.IsGrounded ? Factory.Grounded() : Factory.Falling();
        }
        return null;
    }

    public override void InitializeSubState(PlayerContext context) { }
}