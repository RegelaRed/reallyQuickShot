public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerController _ctx, PlayerStateFactory _factory)
     : base(_ctx, _factory) { InitializeSubState(); }
    public override void EnterState() { }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (Ctx.Controller.isGrounded)
            SwitchStates(Factory.Grounded());
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