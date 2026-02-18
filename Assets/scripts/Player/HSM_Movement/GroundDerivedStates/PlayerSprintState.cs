public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context)
    {
        float speed;
        if (context.Input.AttackHeld && context.Input.AimMode) speed = context.Variables.aimModeSpeed;
        else speed = context.Variables.sprintSpeed;

        context.PlayerMotor.SetSpeed(speed);
        context.PlayerMotor.SetGroundMovementInput(context);
    }
    public override void ExitState(PlayerContext context) { }
    public override void CheckSwitchState(PlayerContext context) { }
    public override void InitializeSubState(PlayerContext context) { }
}
