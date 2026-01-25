public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerController ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory) { }
    public override void EnterState() { }
    public override void Tick()
    {

    }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
