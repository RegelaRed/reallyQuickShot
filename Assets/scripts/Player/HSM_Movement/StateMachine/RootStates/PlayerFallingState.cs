public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerController _ctx, PlayerStateFactory _factory)
     : base(_ctx, _factory) { }
    public override void EnterState(PlayerContext context)
    {
        context.PlayerMotor.SetGravity(context.Variables.gravity);
    }

    public override void ExitState(PlayerContext context) { }

    public override void UpdateState(PlayerContext context)
    {
        context.PlayerMotor.SetAirMovementInput(context);
        CheckSwitchState(context);
    }

    public override void CheckSwitchState(PlayerContext context)
    {

        if (context.IsGrounded)
            SwitchStates(Factory.Grounded(), context);
        else if (context.InputBuffer.JumpBufferActive && JumpRules.CanJump(context))
            SwitchStates(Factory.Jump(), context);
    }

    public override void InitializeSubState(PlayerContext context) { }
}