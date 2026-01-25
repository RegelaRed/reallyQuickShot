public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory) { }
    public override void EnterState() { }
    public override void Tick() { }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (_ctx.Input.IsJumpPressed)
        {
            SwitchStates(_factory.Jump());
        }
        else if (_ctx.Input.IsDashPressed)
        {
            SwitchStates(_factory.Dash());
        }
    }
    public override void InitializeSubState() { }
}
