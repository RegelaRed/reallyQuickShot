public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.Input.IsMovementPressed && !Ctx.Input.IsSprintPressed)
        {
            SwitchStates(Factory.Walk());
        }
        else if (Ctx.Input.IsMovementPressed && Ctx.Input.IsSprintPressed)
        {
            SwitchStates(Factory.Sprint());
        }
    }
    public override void InitializeSubState() { }
}