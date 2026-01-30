public class PlayerJumpAscending : PlayerBaseState
{
    public PlayerJumpAscending(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState() { Ctx.PlayerMotor.SetAirMovementInput(Ctx.Input.CurrentMovementInput); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        // if (Ctx.Controller.velocity.y <= 0)
        //     SwitchStates(Factory.JumpDescending());
    }
    public override void InitializeSubState() { }
}
