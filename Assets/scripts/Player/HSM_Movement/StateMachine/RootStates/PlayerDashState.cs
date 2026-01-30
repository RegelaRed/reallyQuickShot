public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    public override void EnterState()
    {
        Ctx.PlayerMotor.StartDash(Ctx.Orientation.forward, Ctx.Variables.dashDistance, Ctx.Variables.dashDuration);
    }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.PlayerMotor.IsDashing)
            return;

        if (Ctx.IsOnGround)
            SwitchStates(Factory.Grounded());
        else
            SwitchStates(Factory.Falling());
    }
    public override void InitializeSubState() { }
}
