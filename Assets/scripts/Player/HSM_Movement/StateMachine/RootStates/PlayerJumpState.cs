public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    public override void EnterState()
    {
        Ctx.PlayerMotor.SetGravity(Ctx.JumpGravity);
        if (Ctx.PlayerMotor.CurrentSpeed <= 0f)
        {
            if (Ctx.Input.SprintToggle)
                Ctx.PlayerMotor.SetSpeed(Ctx.Variables.sprintSpeed);
            else
                Ctx.PlayerMotor.SetSpeed(Ctx.Variables.walkSpeed);
        }
        Ctx.PlayerMotor.SetUpwardVelocity(Ctx.InitialJumpVelocity);
        InitializeSubState();
    }
    public override void UpdateState()
    {
        Ctx.PlayerMotor.SetAirMovementInput(Ctx.Input.CurrentMovementInput);
        CheckSwitchState();
        UpdateSubstate();
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
        else if (Ctx.CanDash && Ctx.Input.IsDashPressedThisFrame)
        {
            Ctx.DashConsume();
            SwitchStates(Factory.Dash());
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
