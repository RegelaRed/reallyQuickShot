
public class PlayerStateFactory
{
    PlayerController _ctx;
    public PlayerStateFactory(PlayerController _currentContext) { _ctx = _currentContext; }
    //Root States
    public PlayerBaseState Grounded()
    {
        var state = new PlayerGroundedState(_ctx, this);
        state.IsRootState = true;
        return state;
    }
    public PlayerBaseState Falling()
    {
        var state = new PlayerFallingState(_ctx, this);
        state.IsRootState = true;
        return state;
    }
    public PlayerBaseState Jump()
    {
        var state = new PlayerJumpState(_ctx, this);
        state.IsRootState = true;
        return state;
    }
    public PlayerBaseState Dash()
    {
        var state = new PlayerDashState(_ctx, this);
        state.IsRootState = true;
        return state;
    }

    //Sub States
    public PlayerBaseState Idle() { return new PlayerIdleState(_ctx, this); }
    public PlayerBaseState Walk() { return new PlayerWalkState(_ctx, this); }
    public PlayerBaseState Sprint() { return new PlayerSprintState(_ctx, this); }
    public PlayerBaseState JumpAscending() { return new PlayerJumpAscending(_ctx, this); }
    public PlayerBaseState JumpDescending() { return new PlayerJumpDescending(_ctx, this); }
}