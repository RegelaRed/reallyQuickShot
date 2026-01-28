public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.PlayerMotor.SetHorizontalVelocity(Ctx.Input.CurrentMovementInput, Ctx.Variables.sprintSpeed);
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!Ctx.Input.IsMovementPressed && !Ctx.Input.IsSprintPressed)
        {
            SwitchStates(Factory.Idle());
        }
        else if (Ctx.Input.IsMovementPressed && !Ctx.Input.IsSprintPressed)
        {
            SwitchStates(Factory.Walk());
        }
    }
    public override void InitializeSubState() { }
}
