using Unity.VisualScripting;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController _ctx, PlayerStateFactory _factory) : base(_ctx, _factory)
    { }
    // ------------ Enter State ------------
    public override void EnterState(PlayerContext context)
    {
        context.PlayerMotor.SetGravity(context.Variables.gravity);
        InitializeSubState(context);
    }
    // ------------ Enter State ------------
    public override void ExitState(PlayerContext context)
    {
        context.InputBuffer.SetCyoteTime(context.Variables.jumpBufferTime);
    }

    public override void UpdateState(PlayerContext context)
    {
        CheckSwitchState(context);
        UpdateSubstate(context);
    }

    public override void CheckSwitchState(PlayerContext context)
    {
        // ------------ Jump ------------
        if (context.InputBuffer.JumpBufferActive && JumpRules.CanJump(context))
        {
            SwitchStates(Factory.Jump(), context);
        }
        // ------------ Dash ------------
        else if (context.InputBuffer.DashBufferActive && DashRules.CanDash(context))
        {
            DashRules.Consume(context);

            SwitchStates(Factory.Dash(), context);
        }
        else if (!context.IsGrounded)
        {
            SwitchStates(Factory.Falling(), context);
        }
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
