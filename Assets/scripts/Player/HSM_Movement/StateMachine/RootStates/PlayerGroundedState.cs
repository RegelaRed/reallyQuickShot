using Unity.VisualScripting;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController _ctx, PlayerStateFactory _factory) : base(_ctx, _factory)
    { }
    public override void EnterState(ref PlayerContext context)
    {
        Ctx.PlayerMotor.SetGravity(context.Variables.gravity);
        InitializeSubState(ref context);
    }
    public override void ExitState(ref PlayerContext context) { Ctx.TimeLeftOnGround = Ctx.Variables.jumpBufferTime; }
    public override void UpdateState(ref PlayerContext context)
    {
        CheckSwitchState(ref context);
        UpdateSubstate(ref context);
    }
    public override void CheckSwitchState(ref PlayerContext context)
    {
        if (context.Input.JumpPressed && (Ctx.Input.JumpBufferActive || Ctx.Input.IsJumpPressedThisFrame))
        {
            SwitchStates(Factory.Jump(), ref context);
        }
        else if (Ctx.CanDash && (Ctx.Input.DashBufferActive || Ctx.Input.IsDashPressedThisFrame))
        {
            Ctx.DashConsume();
            SwitchStates(Factory.Dash(), ref context);
        }
        else if (!Ctx.IsOnGround)
        {
            SwitchStates(Factory.Falling(), ref context);
        }
    }
    private void UpdateSubstate(ref PlayerContext context)
    {
        PlayerBaseState desiredState = GetDesiredState();
        if (CurrentSubState?.GetType() != desiredState?.GetType())
        {
            SetSubState(desiredState, ref context);
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

    public override void InitializeSubState(ref PlayerContext context)
    {
        PlayerBaseState desiredState = GetDesiredState();
        if (desiredState != null)
            SetSubState(desiredState, ref context);
    }
}
