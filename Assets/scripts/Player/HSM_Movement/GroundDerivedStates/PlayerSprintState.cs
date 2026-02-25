public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    public override void EnterState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context)
    {
        float speed;
        if (context.Input.AttackHeld && context.Input.AimMode) speed = context.Variables.aimModeSpeed;
        else speed = context.Variables.sprintSpeed;

        context.CurrentSpeed = speed;
        Motor.SetGroundMovementInput(context);
    }
    public override void ExitState(PlayerContext context) { }
    public override PlayerBaseState CheckSwitchState(PlayerContext context) { return this; }
    public override void InitializeSubState(PlayerContext context) { }
}
