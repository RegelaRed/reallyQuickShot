public class PlayerJumpDescending : PlayerBaseState
{
    public PlayerJumpDescending(PlayerController ctx, PlayerStateFactory stateFactory) : base(ctx, stateFactory)
    { }
    private float _jumpExitTimer;
    public override void EnterState(PlayerContext context)
    {
        _jumpExitTimer = context.TimeToApex;
    }
    public override void ExitState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context)
    {
        if (_jumpExitTimer >= 0f)
            _jumpExitTimer -= context.DeltaTime;

        CheckSwitchState(context);
    }
    public override void CheckSwitchState(PlayerContext context)
    {
        if (_jumpExitTimer <= 0f)
            SwitchStates(Factory.Falling(), context);
        if (context.IsGrounded)
            SwitchStates(Factory.Grounded(), context);
    }
    public override void InitializeSubState(PlayerContext context) { }
}
