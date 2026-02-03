public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void UpdateState()
    {
        Ctx.PlayerMotor.CheckAimModeSpeed(Ctx.Variables.sprintSpeed);
        Ctx.PlayerMotor.SetGroundMovementInput(Ctx.Input.CurrentMovementInput);
    }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
