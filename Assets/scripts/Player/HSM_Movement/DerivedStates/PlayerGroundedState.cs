using Unity.VisualScripting;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    // ------------ Enter State ------------
    public override void EnterState(PlayerContext context)
    {
        context.CurrentGravity = context.JumpGravity;
        context.CurrentSpeed = context.Variables.walkSpeed;
        InitializeSubState(context);
    }
    // ------------ Enter State ------------
    public override void ExitState(PlayerContext context)
    {
        if (context.IsGrounded)
            context.InputBuffer.SetCyoteTime(context.Variables.jumpBufferTime);
    }

    public override void UpdateState(PlayerContext context)
    {
        UpdateSubstate(context);
    }

    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        // ------------ Jump ------------
        if (context.InputBuffer.JumpBufferActive && JumpRules.CanJump(context))
        {
            return Factory.Jump();
        }
        // ------------ Dash ------------
        else if (context.InputBuffer.DashBufferActive && DashRules.CanDash(context))
        {
            DashRules.Consume(context);

            return Factory.Dash();
        }
        else if (!context.IsGrounded)
        {
            return Factory.Falling();
        }
        return this;
    }

    private void UpdateSubstate(PlayerContext context)
    {
        PlayerBaseState desiredState = GetDesiredState(context);
        if (CurrentSubState?.GetType() != desiredState?.GetType())
        {
            SetSubState(desiredState, context);
        }
    }

    private PlayerBaseState GetDesiredState(PlayerContext context)
    {
        if (context.Input.MovePressed && context.Input.SprintToggle)
            return Factory.Sprint();
        else if (context.Input.MovePressed)
            return Factory.Walk();
        else
            return Factory.Idle();
    }

    public override void InitializeSubState(PlayerContext context)
    {
        PlayerBaseState desiredState = GetDesiredState(context);
        if (desiredState != null)
            SetSubState(desiredState, context);
    }
}
