public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory)
    { _ctx.PlayerMotor.SetGravity(_ctx.Variables.gravity); }
    public override void EnterState() { InitializeSubState(); }
    public override void UpdateState()
    {
        CheckSwitchState();
        UpdateSubstate();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.Input.IsJumpPressed && Ctx.Input.IsJumpPressedThisFrame)
        {
            SwitchStates(Factory.Jump());
        }
        else if (Ctx.CanDash && Ctx.Input.IsDashPressedThisFrame)
        {
            Ctx.DashTime = Ctx.Variables.dashRegenTime;
            SwitchStates(Factory.Dash());
        }
        else if (!Ctx.IsOnGround)
        {
            SwitchStates(Factory.Falling());
        }
    }

    private void UpdateSubstate()
    {
        PlayerBaseState desiredState = GetDesiredState();
        if (CurrentSubState?.GetType() != desiredState?.GetType())
        {
            CurrentSubState.ExitState();
            SetSubState(desiredState);
        }
    }
    private PlayerBaseState GetDesiredState()
    {

        if (Ctx.Input.IsMovementPressed && Ctx.Input.SprintToggle)
            return Factory.Sprint();
        else if (Ctx.Input.IsMovementPressed)
            return Factory.Walk();
        else
            return Factory.Idle();
    }

    public override void InitializeSubState()
    {
        PlayerBaseState desiredState = GetDesiredState();
        if (desiredState != null)
            SetSubState(desiredState);
    }
}
