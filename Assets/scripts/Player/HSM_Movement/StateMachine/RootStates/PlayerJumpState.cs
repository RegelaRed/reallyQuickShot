using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    bool _jumpCutthisFrame;
    float _jumpExitTimer;
    public override void EnterState()
    {
        _jumpCutthisFrame = false;
        _jumpExitTimer = Ctx.Variables.maxJumpTime;
        Ctx.PlayerMotor.SetGravity(Ctx.JumpGravity);

        if (Ctx.PlayerMotor.CurrentSpeed <= 0f)
            Ctx.PlayerMotor.SetSpeed(Ctx.Input.SprintToggle ? Ctx.Variables.sprintSpeed : Ctx.Variables.walkSpeed);

        Ctx.PlayerMotor.SetUpwardVelocity(Ctx.InitialJumpVelocity);

        InitializeSubState();
    }
    public override void UpdateState()
    {
        if (_jumpExitTimer > 0f) _jumpExitTimer -= Time.deltaTime;
        Ctx.PlayerMotor.SetAirMovementInput(Ctx.Input.CurrentMovementInput);
        CutJump();
        CheckSwitchState();
        UpdateSubstate();
    }

    private void CutJump()
    {
        if (!_jumpCutthisFrame && !Ctx.Input.IsJumpPressed && Ctx.PlayerMotor.VerticalVelocity > 0)
        {
            Ctx.PlayerMotor.SetUpwardVelocity(Ctx.PlayerMotor.VerticalVelocity * 0.5f);
            _jumpCutthisFrame = true;
        }
    }


    public override void ExitState()
    {
        Ctx.TimeLeftOnGround = 0.1f;
    }
    public override void CheckSwitchState()
    {
        if (_jumpExitTimer <= 0.01f)
        {

            if (Ctx.IsOnGround)
                SwitchStates(Factory.Grounded());
            else
                SwitchStates(Factory.Falling());
        }
        else if (Ctx.CanDash && Ctx.Input.IsDashPressedThisFrame)
        {
            Ctx.DashConsume();
            SwitchStates(Factory.Dash());
        }
    }
    public override void InitializeSubState()
    {
        if (Ctx.PlayerMotor.VerticalVelocity > 0)
            SetSubState(Factory.JumpAscending());
    }

    private void UpdateSubstate()
    {
        if (Ctx.PlayerMotor.VerticalVelocity <= 0 && CurrentSubState?.GetType() != typeof(PlayerJumpDescending))
        {
            CurrentSubState?.ExitState();
            SetSubState(Factory.JumpDescending());
        }
    }
}
