public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { IsRootState = true; }

    public override void EnterState()
    {
        Ctx.PlayerMotor.ApplyJumpForce(Ctx.InitialJumpVelocity, Ctx.JumpGravity);
    }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.Controller.isGrounded)
        {
            SwitchStates(Factory.Grounded());
        }
    }
    public override void InitializeSubState() { }
}
