using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
        : base(_ctx, _factory) { }
    bool _jumpCutthisFrame;

    public override void EnterState(PlayerContext context)
    {
        _jumpCutthisFrame = false;
        context.PlayerMotor.SetGravity(context.JumpGravity);
        context.PlayerMotor.SetSpeed(context.Input.SprintToggle ? context.Variables.sprintSpeed : context.Variables.walkSpeed);
        context.PlayerMotor.SetUpwardVelocity(context.InitialJumpVelocity);

        InitializeSubState(context);
    }
    public override void ExitState(PlayerContext context)
    {
        context.PlayerMotor.SetSpeed(
            context.Input.SprintToggle ?
                context.Variables.sprintSpeed :
                context.Variables.walkSpeed
            );
    }
    public override void UpdateState(PlayerContext context)
    {
        context.PlayerMotor.SetAirMovementInput(context);
        CutJump(context);
        CheckSwitchState(context);
    }

    private void CutJump(PlayerContext context)
    {
        if (!_jumpCutthisFrame && !context.Input.JumpPressed && context.PlayerMotor.VerticalVelocity > 0)
        {
            context.PlayerMotor.SetUpwardVelocity(context.PlayerMotor.VerticalVelocity * 0.5f);
            _jumpCutthisFrame = true;
        }
    }

    public override void CheckSwitchState(PlayerContext context)
    {
        if (DashRules.CanDash(context) && context.Input.DashPressed)
        {
            DashRules.Consume(context);
            SwitchStates(Factory.Dash(), context);
        }
    }
    public override void InitializeSubState(PlayerContext context)
    {
        if (context.PlayerMotor.VerticalVelocity > 0)
            SetSubState(Factory.JumpAscending(), context);
    }
}
