/// <summary>
/// Responsible for creating and configuring player state instances.<para/>
/// Provides factory methods for both root and sub states.
/// </summary>
public class PlayerStateFactory
{
    private PlayerController _ctx;
    public PlayerStateFactory(PlayerController _currentContext) { _ctx = _currentContext; }
    //Super States
    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_ctx, this) { IsSuperState = true };
    }
    public PlayerBaseState Falling()
    {
        return new PlayerFallingState(_ctx, this) { IsSuperState = true };
    }
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_ctx, this) { IsSuperState = true };
    }
    public PlayerBaseState Dash()
    {
        return new PlayerDashState(_ctx, this) { IsSuperState = true };
    }

    //Sub States
    public PlayerBaseState Idle() { return new PlayerIdleState(_ctx, this); }
    public PlayerBaseState Walk() { return new PlayerWalkState(_ctx, this); }
    public PlayerBaseState Sprint() { return new PlayerSprintState(_ctx, this); }
    public PlayerBaseState JumpAscending() { return new PlayerJumpAscending(_ctx, this); }
    public PlayerBaseState JumpDescending() { return new PlayerJumpDescending(_ctx, this); }
}