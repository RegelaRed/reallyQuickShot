using Unity.VisualScripting;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerController _ctx, PlayerStateFactory _factory)
     : base(_ctx, _factory) { }
    public override void EnterState()
    {
        Ctx.PlayerMotor.Gravity = Ctx.Variables.gravity;
        Ctx.PlayerMotor.SetSpeed(Ctx.Variables.airMoveSpeed);
    }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.IsOnGround)
            SwitchStates(Factory.Grounded());
    }
    public override void InitializeSubState()
    { }
}