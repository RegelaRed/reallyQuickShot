public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerController _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    public bool IsRootState { get { return _isRootState; } set { _isRootState = value; } }
    public PlayerController Ctx { get { return _ctx; } }
    public PlayerStateFactory Factory { get { return _factory; } }
    public PlayerBaseState CurrentSubState { get { return _currentSubState; } }
    public PlayerBaseState CurrentSuperState { get { return _currentSuperState; } }
    public PlayerBaseState(PlayerController ctx, PlayerStateFactory stateFactory)
    {
        _ctx = ctx;
        _factory = stateFactory;
    }
    public abstract void EnterState(PlayerContext context);
    public abstract void ExitState(PlayerContext context);
    public abstract void UpdateState(PlayerContext context);
    public abstract void CheckSwitchState(PlayerContext context);
    public abstract void InitializeSubState(PlayerContext context);
    protected void SwitchStates(PlayerBaseState newState, PlayerContext context)
    {
        //current state exit
        ExitState(context);

        //new state enter
        newState.EnterState(context);

        if (IsRootState)
        {
            //switch current state context
            _ctx.CurrentMovementState = newState;
        }
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState, context);
    }
    /// <summary>Update SubstatesStates if any</summary>
    public void UpdateStates(PlayerContext context)
    {
        UpdateState(context);
        if (_currentSubState != null)
            _currentSubState.UpdateStates(context);
    }
    /// <summary>Exit All SubStates if any</summary>
    public void ExitStates(PlayerContext context)
    {
        ExitState(context);
        if (_currentSubState != null)
            _currentSubState.ExitStates(context);
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState, PlayerContext context)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        newSubState.EnterState(context);
    }
}
/// Template
/// public *StateNeme* (PlayerController _ctx, PlayerStateFactory _factory)
/// : base(_ctx, _factory) { }
/// public override void EnterState() { }
/// public override void UpdateState() { }
/// public override void ExitState() { }
/// public override void CheckSwitchState() { }
/// public override void InitializeSubState() { }