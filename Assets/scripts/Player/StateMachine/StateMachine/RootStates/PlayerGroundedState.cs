public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory)
    {
        IsRootState = true;
        _ctx.PlayerMotor.SetGravity(_ctx.Variables.gravity);
    }
    public override void EnterState() { InitializeSubState(); }
    public override void UpdateState()
    {
        CheckSwitchState();
        UpdateSubstate();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.Input.IsJumpPressed && !Ctx.RequestJumpAgain)
        {
            SwitchStates(Factory.Jump());
        }
        else if (Ctx.Input.SprintToggle && !Ctx.RequestDashAgain)
        {
            SwitchStates(Factory.Dash());
        }
        else if (!Ctx.Controller.isGrounded)
        {
            SwitchStates(Factory.Falling());
        }
    }

    private void UpdateSubstate()
    {
        PlayerBaseState desiredState = GetDesiredState();
        if (CurrentSubState?.GetType() != desiredState.GetType())
        {
            CurrentSubState?.ExitState();
            SetSubState(desiredState);
            desiredState.EnterState();
        }
    }
    private PlayerBaseState GetDesiredState()
    {
        if (!Ctx.Input.IsMovementPressed && !Ctx.Input.SprintToggle)
            return Factory.Idle();
        else if (Ctx.Input.IsMovementPressed && !Ctx.Input.SprintToggle)
            return Factory.Walk();
        else if (Ctx.Input.IsMovementPressed && Ctx.Input.SprintToggle)
            return Factory.Sprint();
        return null;
    }

    public override void InitializeSubState()
    {
        if (!Ctx.Input.IsMovementPressed && !Ctx.Input.SprintToggle)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.Input.IsMovementPressed && !Ctx.Input.SprintToggle)
        {
            SetSubState(Factory.Walk());
        }
        else if (Ctx.Input.IsMovementPressed && Ctx.Input.SprintToggle)
        {
            SetSubState(Factory.Sprint());
        }
    }
}
