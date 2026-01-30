public class PlayerJumpDescending : PlayerBaseState
{
    public PlayerJumpDescending(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState() { Ctx.PlayerMotor.SetAirMovementInput(Ctx.Input.CurrentMovementInput); }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
