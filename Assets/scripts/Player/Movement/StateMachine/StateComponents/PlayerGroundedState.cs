public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory) { }
    public override void EnterState() { }
    public override void Update() { }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
