public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { Ctx.PlayerMotor.SetSpeed(Ctx.Variables.sprintSpeed); }
    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.PlayerMotor.SetGroundMovementInput(Ctx.Input.CurrentMovementInput);
    }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
