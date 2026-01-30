public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    public override void EnterState()
    {
        if (Ctx.Input.IsAiming)
            Ctx.PlayerMotor.StartDash(Ctx.Variables.dashDistance, Ctx.Variables.dashDuration, Ctx.Orientation.forward);
        else
            Ctx.PlayerMotor.StartDash(Ctx.Variables.dashDistance, Ctx.Variables.dashDuration, Ctx.FaceDirection.forward);
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
