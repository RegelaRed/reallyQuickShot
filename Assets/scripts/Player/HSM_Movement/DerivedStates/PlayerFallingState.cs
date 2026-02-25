using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    public override void EnterState(PlayerContext context)
    {
        context.CurrentGravity = context.JumpGravity;
    }

    public override void ExitState(PlayerContext context) { }

    public override void UpdateState(PlayerContext context)
    {
        Motor.SetAirMovementInput(context);
        CheckSwitchState(context);
    }

    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        if (context.IsGrounded)
            return Factory.Grounded();
        else if (context.InputBuffer.JumpBufferActive && JumpRules.CanJump(context))
            return Factory.Jump();
        return this;
    }

    public override void InitializeSubState(PlayerContext context) { }
}