public class PlayerJumpAscending : PlayerBaseState
{
    public PlayerJumpAscending(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context) { }
    public override void ExitState(PlayerContext context) { }
    public override void CheckSwitchState(PlayerContext context)
    {
        // if (Ctx.Controller.velocity.y <= 0)
        //     SwitchStates(Factory.JumpDescending());
    }
    public override void InitializeSubState(PlayerContext context) { }
}
