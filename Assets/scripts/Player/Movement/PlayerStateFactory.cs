
public class PlayerStateFactory
{
    PlayerController _ctx;
    public PlayerStateFactory(PlayerController _currentContext)
    {
        _ctx = _currentContext;
    }
    public PlayerBaseState Idle() { return new PlayerIdleState(_ctx, this); }
    public PlayerBaseState Walk() { return new PlayerWalkState(_ctx, this); }
    public PlayerBaseState Sprint() { return new PlayerSprintState(_ctx, this); }
    public PlayerBaseState Jump() { return new PlayerJumpState(_ctx, this); }
    public PlayerBaseState Dash() { return new PlayerDashState(_ctx, this); }
    public PlayerBaseState Grounded() { return new PlayerGroundedState(_ctx, this); }
}