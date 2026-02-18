using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
        : base(_ctx, _factory) { }
    bool _jumpCutthisFrame;
    float _jumpExitTimer;
    public override void EnterState(PlayerContext context)
    {
        _jumpCutthisFrame = false;
        _jumpExitTimer = context.Variables.maxJumpTime;
        context.PlayerMotor.SetGravity(context.JumpGravity);

        if (context.PlayerMotor.CurrentSpeed <= 0f)
            context.PlayerMotor.SetSpeed(context.Input.SprintToggle ? context.Variables.sprintSpeed : context.Variables.walkSpeed);

        context.PlayerMotor.SetUpwardVelocity(context.InitialJumpVelocity);

        InitializeSubState(context);
    }
    public override void UpdateState(PlayerContext context)
    {
        if (_jumpExitTimer > 0f) _jumpExitTimer -= Time.deltaTime;
        context.PlayerMotor.SetAirMovementInput(context);
        CutJump(context);
        CheckSwitchState(context);
        UpdateSubstate(context);
    }

    private void CutJump(PlayerContext context)
    {
        if (!_jumpCutthisFrame && !context.Input.JumpPressed && context.PlayerMotor.VerticalVelocity > 0)
        {
            context.PlayerMotor.SetUpwardVelocity(context.PlayerMotor.VerticalVelocity * 0.5f);
            _jumpCutthisFrame = true;
        }
    }

    public override void ExitState(PlayerContext context) { }
    public override void CheckSwitchState(PlayerContext context)
    {
        if (_jumpExitTimer <= 0.01f)
        {
            if (context.IsGrounded)
                SwitchStates(Factory.Grounded(), context);
            else
                SwitchStates(Factory.Falling(), context);
        }
        else if (DashRules.CanDash(context) && context.Input.DashPressed)
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

    private void UpdateSubstate(PlayerContext context)
    {
        if (context.PlayerMotor.VerticalVelocity <= 0 && CurrentSubState?.GetType() != typeof(PlayerJumpDescending))
        {
            CurrentSubState?.ExitState(context);
            SetSubState(Factory.JumpDescending(), context);
        }
    }
}
