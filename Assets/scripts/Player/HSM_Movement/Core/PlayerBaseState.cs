public abstract class PlayerBaseState
{
    #region References
    private bool _isRootState = false;
    /// <summary>PlayerController Reference</summary>
    // private PlayerController _ctx;
    private PlayerStateFactory _factory;
    private PlayerMotor _playerMotor;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    public bool IsSuperState { get => _isRootState; set => _isRootState = value; }

    /// <summary>PlayerStateFactory Reference</summary>
    public PlayerStateFactory Factory => _factory;

    /// <summary>PlayerMotor Reference </summary>
    public PlayerMotor Motor => _playerMotor;
    public PlayerBaseState CurrentSubState => _currentSubState;
    public PlayerBaseState CurrentSuperState => _currentSuperState;

    #endregion

    #region Constructors
    public PlayerBaseState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    {
        _factory = stateFactory;
        _playerMotor = playerMotor;
    }
    #endregion

    #region Base State Functions

    /// <summary>
    /// Called once when the state is Entered
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public abstract void EnterState(PlayerContext context);
    /// <summary>
    /// Called once when the state is Ended
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public abstract void ExitState(PlayerContext context);
    /// <summary>
    /// Update this state's behaviour each frame
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public abstract void UpdateState(PlayerContext context);
    /// <summary>
    /// Evaluate state switch conditions and switches to new state
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public abstract PlayerBaseState CheckSwitchState(PlayerContext context);
    /// <summary>
    /// Initialize default substate for this state
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public abstract void InitializeSubState(PlayerContext context);
    /// <summary>
    /// Transitions from the current state to a new state,
    /// correctly handling root and substate hierarchy.
    /// </summary>
    /// <param name="newState">NewState to transition into</param>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public void SwitchStates(PlayerBaseState newState, PlayerContext context, PlayerController ctx)
    {
        //current state exit
        ExitState(context);

        //new state enter
        newState.EnterState(context);

        if (IsSuperState)
        {
            //switch current state context
            ctx.CurrentMovementState = newState;
        }
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState, context);
    }

    /// <summary>
    /// Update this State and all active Substates 
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public void UpdateStates(PlayerContext context)
    {
        UpdateState(context);
        if (_currentSubState != null)
            _currentSubState.UpdateStates(context);
    }
    /// <summary>
    /// Exit this State and all active Substates 
    /// </summary>
    /// <param name="context">Provides runtime data and input information required by player states</param>
    public void ExitStates(PlayerContext context)
    {
        ExitState(context);
        if (_currentSubState != null)
            _currentSubState.ExitStates(context);
    }
    /// <summary>
    /// Assign Superstate for this Substate
    /// Intended for internal use during state transitions
    /// </summary>
    /// <param name="newSuperState">NewSuperState to transition into</param>
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    /// <summary>
    /// Assigns and Initializes a substate for the current State
    /// Intended for internal use during state transitions
    /// </summary>
    /// <param name="newSubState">NewSubState to transition into</param>
    /// <param name="context">Provides runtime data and input information required by player states.</param>
    protected void SetSubState(PlayerBaseState newSubState, PlayerContext context)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        newSubState.EnterState(context);
    }
    #endregion
}