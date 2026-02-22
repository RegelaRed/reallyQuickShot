public class PlayerJumpAscending : PlayerBaseState
{
    public PlayerJumpAscending(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context)
    {
        CheckSwitchState(context);
    }
    public override void ExitState(PlayerContext context) { }
    public override void CheckSwitchState(PlayerContext context)
    {
        if (context.PlayerMotor.VerticalVelocity <= 0)
        {
            SwitchStates(Factory.JumpDescending(), context);
        }
    }
    public override void InitializeSubState(PlayerContext context) { }
}
