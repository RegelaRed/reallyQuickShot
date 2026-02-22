public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController _ctx, PlayerStateFactory _factory) : base(_ctx, _factory)
    { }
    public override void EnterState(PlayerContext context)
    {
        if (CurrentSubState != null)
            CurrentSubState.ExitStates(context);

        context.PlayerMotor.SetGravity(context.DashGravity);
        context.PlayerMotor.StartDash(context.Variables.dashDistance, context.Variables.dashDuration, context.DashDirection);
        context.PlayerMotor.SetUpwardVelocity(context.InitialDashVelocity);
    }
    public override void ExitState(PlayerContext context)
    {
        // context.InputBuffer.ConsumeJump();
        context.PlayerMotor.SetGravity(context.JumpGravity);
        if (!context.IsGrounded)
            context.PlayerMotor.SetSpeed(context.Variables.airMoveSpeed);
    }
    public override void UpdateState(PlayerContext context) { CheckSwitchState(context); }
    public override void CheckSwitchState(PlayerContext context)
    {
        if (context.PlayerMotor.IsDashing)
            return;

        if (context.IsGrounded)
            SwitchStates(Factory.Grounded(), context);
        else
            SwitchStates(Factory.Falling(), context);
    }
    public override void InitializeSubState(PlayerContext context) { }
}
