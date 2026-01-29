public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    public override void EnterState()
    {
        Ctx.PlayerMotor.Gravity = Ctx.JumpGravity;
        Ctx.PlayerMotor.SetSpeed(Ctx.Variables.airMoveSpeed);
        Ctx.PlayerMotor.SetJumpVelocity(Ctx.InitialJumpVelocity);
        InitializeSubState();
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        UpdateSubstate();
        Ctx.PlayerMotor.SetMovementInput(Ctx.Input.CurrentMovementInput);
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.IsOnGround)
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

    private void UpdateSubstate()
    {
        if (Ctx.PlayerMotor.VerticalVelocity > 0 && CurrentSubState?.GetType() != typeof(PlayerJumpAscending))
        {
            CurrentSubState?.ExitState();
            SetSubState(Factory.JumpAscending());
        }
        else if (CurrentSubState?.GetType() != typeof(PlayerJumpDescending))
        {
            CurrentSubState?.ExitState();
            SetSubState(Factory.JumpAscending());
        }
    }
}
