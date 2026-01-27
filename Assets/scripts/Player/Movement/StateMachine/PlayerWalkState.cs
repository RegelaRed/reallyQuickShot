public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!_ctx.Input.IsMovementPressed && !_ctx.Input.IsSprintPressed)
        {
            SwitchStates(_factory.Idle());
        }
        else if (_ctx.Input.IsMovementPressed && _ctx.Input.IsSprintPressed)
        {
            SwitchStates(_factory.Sprint());
        }
    }
    public override void InitializeSubState() { }
}
