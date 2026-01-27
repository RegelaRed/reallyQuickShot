public abstract class PlayerBaseState
{
    protected PlayerController _ctx;
    protected PlayerStateFactory _factory;
    public PlayerBaseState(PlayerController ctx, PlayerStateFactory stateFactory)
    {
        _ctx = ctx;
        _factory = stateFactory;
    }
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;
    /// <summary>
    /// Set Enter animator or one time trigger logic into this
    /// </summary>
    public abstract void EnterState();
    public abstract void UpdateState();
    /// <summary>
    /// Set Exit animator or one time trigger logic into this
    /// </summary>
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();

    protected void SwitchStates(PlayerBaseState newState)
    {
        //current state exit
        ExitState();
        //new state enter
        newState.EnterState();

        //switch current state context
        _ctx.CurrentState = newState;
    }
    protected void UpdateStates() { }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
