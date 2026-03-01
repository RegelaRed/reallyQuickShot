/// <summary>
/// Responsible for creating and caching player movement state instances.
/// <para/>
/// States are created once and reused to avoid runtime allocations and
/// maintain persistent state data (timers, buffers, etc).
/// <para/>
/// Provides access to both super states and substates.
/// </summary>
public class PlayerStateFactory
{
    // super States

    private PlayerGroundedState _ground;
    private PlayerFallingState _fall;
    private PlayerJumpState _jump;
    private PlayerDashState _dash;

    // Sub States

    private PlayerIdleState _idle;
    private PlayerMoveState _walk;
    private PlayerMoveState _sprint;

    /// <summary>
    /// Initializes and caches all player movement states.
    /// <para/>
    /// Each state is instantiated once and reused for the lifetime
    /// of the PlayerMotor.
    /// </summary>
    public PlayerStateFactory(PlayerMotor playerMotor)
    {
        // Super states
        _ground = new PlayerGroundedState(this, playerMotor) { IsSuperState = true };
        _fall = new PlayerFallingState(this, playerMotor) { IsSuperState = true };
        _jump = new PlayerJumpState(this, playerMotor) { IsSuperState = true };
        _dash = new PlayerDashState(this, playerMotor) { IsSuperState = true };

        // Sub states
        _idle = new PlayerIdleState(this, playerMotor);

        // Third parameter indicates sprint mode
        _walk = new PlayerMoveState(this, playerMotor, false);
        _sprint = new PlayerMoveState(this, playerMotor, true);
    }



    public PlayerBaseState Grounded() => _ground;
    public PlayerBaseState Falling() => _fall;
    public PlayerBaseState Jump() => _jump;
    public PlayerBaseState Dash() => _dash;

    // Sub States

    public PlayerBaseState Idle() => _idle;
    public PlayerBaseState Walk() => _walk;
    public PlayerBaseState Sprint() => _sprint;
}