public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    public override void EnterState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context) { }
    public override void ExitState(PlayerContext context) { }
    public override PlayerBaseState CheckSwitchState(PlayerContext context) { return this; }
    public override void InitializeSubState(PlayerContext context) { }
}