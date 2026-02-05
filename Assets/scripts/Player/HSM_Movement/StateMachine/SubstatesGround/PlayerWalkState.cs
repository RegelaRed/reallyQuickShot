public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState() { }
    public override void ExitState() { }
    public override void UpdateState()
    {
        Ctx.PlayerMotor.CheckAimModeSpeed(Ctx.Variables.walkSpeed);
        Ctx.PlayerMotor.SetGroundMovementInput(Ctx.Input.CurrentMovementInput);
    }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}