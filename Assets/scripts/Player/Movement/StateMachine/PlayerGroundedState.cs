public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory)
    {
        IsRootState = true;
        _ctx.PlayerMotor.SetGravity(_ctx.Variables.gravity);
        InitializeSubState();
    }
    public override void EnterState() { }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.Input.IsJumpPressed)
        {
            SwitchStates(Factory.Jump());
        }
        else if (Ctx.Input.IsDashPressed)
        {
            SwitchStates(Factory.Dash());
        }
    }
    public override void InitializeSubState()
    {
        if (!Ctx.Input.IsMovementPressed && !Ctx.Input.IsSprintPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.Input.IsMovementPressed && !Ctx.Input.IsSprintPressed)
        {
            SetSubState(Factory.Walk());
        }
        else if (Ctx.Input.IsMovementPressed && Ctx.Input.IsSprintPressed)
        {
            SetSubState(Factory.Sprint());
        }
    }
}
