public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    public override void EnterState()
    {
        Ctx.PlayerMotor.SetGravity(Ctx.JumpGravity);
        Ctx.PlayerMotor.SetJumpVelocity(Ctx.InitialJumpVelocity);
        InitializeSubState();
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        UpdateSubstate();
        Ctx.PlayerMotor.SetMovementInput(Ctx.Input.CurrentMovementInput);
    }
    public override void ExitState()
    {
        Ctx.TimeLeftOnGround = 0.1f;
    }
    public override void CheckSwitchState()
    {
        if (Ctx.IsOnGround)
        {
            SwitchStates(Factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
        if (Ctx.PlayerMotor.VerticalVelocity > 0)
            SetSubState(Factory.JumpAscending());
    }

    private void UpdateSubstate()
    {
        if (Ctx.PlayerMotor.VerticalVelocity <= 0 && CurrentSubState?.GetType() != typeof(PlayerJumpDescending))
        {
            CurrentSubState?.ExitState();
            SetSubState(Factory.JumpDescending());
        }
    }
}
