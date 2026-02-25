using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    bool _jumpCutthisFrame;

    public PlayerJumpState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    public override void EnterState(PlayerContext context)
    {
        _jumpCutthisFrame = false;
        context.CurrentSpeed = context.Input.SprintToggle ? context.Variables.sprintSpeed : context.Variables.walkSpeed;
        Motor.SetUpwardVelocity(context.InitialJumpVerticalVelocity);

        InitializeSubState(context);
    }
    public override void ExitState(PlayerContext context)
    {
        context.CurrentSpeed =
            context.Input.SprintToggle ?
            context.Variables.sprintSpeed : context.Variables.walkSpeed;
    }
    public override void UpdateState(PlayerContext context)
    {
        Motor.SetAirMovementInput(context);
        CutJump(context);
        CheckSwitchState(context);
    }

    private void CutJump(PlayerContext context)
    {
        if (!_jumpCutthisFrame && !context.Input.JumpPressed && Motor.VerticalVelocity > 0)
        {
            Motor.SetUpwardVelocity(Motor.VerticalVelocity * 0.5f);
            _jumpCutthisFrame = true;
        }
    }

    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        if (DashRules.CanDash(context) && context.Input.DashPressed)
        {
            DashRules.Consume(context);
            return Factory.Dash();
        }
        return this;
    }

    public override void InitializeSubState(PlayerContext context) { }
}
