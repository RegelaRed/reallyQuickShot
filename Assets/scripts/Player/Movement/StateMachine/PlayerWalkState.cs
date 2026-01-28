public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.PlayerMotor.SetHorizontalVelocity(Ctx.Input.CurrentMovementInput, Ctx.Variables.walkSpeed);
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!Ctx.Input.IsMovementPressed && !Ctx.Input.IsSprintPressed)
        {
            SwitchStates(Factory.Idle());
        }
        else if (Ctx.Input.IsMovementPressed && Ctx.Input.IsSprintPressed)
        {
            SwitchStates(Factory.Sprint());
        }
    }
    public override void InitializeSubState() { }
}