public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    public override void EnterState()
    {
        Ctx.RequestJumpAgain = false;
        Ctx.PlayerMotor.ApplyJumpForce(Ctx.InitialJumpVelocity, Ctx.JumpGravity);
        InitializeSubState();
    }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState()
    {
        if (Ctx.Input.IsJumpPressed)
            Ctx.RequestJumpAgain = true;
    }
    public override void CheckSwitchState()
    {
        if (Ctx.Controller.isGrounded)
        {
            SwitchStates(Factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
        if (Ctx.Controller.velocity.y > 0)
            SetSubState(Factory.JumpAscending());
        else
            SetSubState(Factory.JumpDescending());
    }
}
