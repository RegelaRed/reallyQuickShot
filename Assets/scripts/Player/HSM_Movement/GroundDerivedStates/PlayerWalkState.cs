public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }
    public override void EnterState(PlayerContext context) { }
    public override void ExitState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context)
    {
        float speed;
        if (context.Input.AttackHeld && context.Input.AimMode) speed = context.Variables.aimModeSpeed;
        else speed = context.Variables.walkSpeed;

        context.CurrentSpeed = speed;
        Motor.SetGroundMovementInput(context);
    }
    public override PlayerBaseState CheckSwitchState(PlayerContext context) { return this; }
    public override void InitializeSubState(PlayerContext context) { }
}