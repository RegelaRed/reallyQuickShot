/// <summary>
/// Base class for hierarchical player movement states.
///
/// Supports a two-layer state structure consisting of:
/// - Super states (grounded, jumping, falling, dash)
/// - Substates (idle, walk, sprint)
///
/// Handles lifecycle propagation and hierarchical transitions.
/// </summary>
public abstract class PlayerBaseState
{
    #region References
    private bool _isRootState = false;
    private PlayerStateFactory _factory;
    private PlayerMotor _playerMotor;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    public bool IsSuperState { get => _isRootState; set => _isRootState = value; }
    public PlayerStateFactory Factory => _factory;
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

    #region Abstract Api

    /// <summary>
    /// Called once when the state becomes active.
    /// </summary>
    public abstract void EnterState(PlayerContext context);

    /// <summary>
    /// Called once when the state is deactivated.
    /// </summary>
    public abstract void ExitState(PlayerContext context);

    /// <summary>
    /// Executes this state's behavior for the current frame.
    /// </summary>
    public abstract void UpdateState(PlayerContext context);

    /// <summary>
    /// Evaluates transition conditions and returns the next state.
    /// Returning this state means no transition occurs.
    /// </summary>
    public abstract PlayerBaseState CheckSwitchState(PlayerContext context);

    /// <summary>
    /// Initializes the default substate when this state becomes active.
    /// </summary>
    public abstract void InitializeSubState(PlayerContext context);

    #endregion

    #region Public Api

    /// <summary>
    /// Performs a state transition while preserving the hierarchy.
    ///
    /// ASSUMES:
    /// - ExitStates() has not already been called this frame
    /// - newState belongs to the same factory
    ///
    /// GUARANTEES:
    /// - Current state hierarchy is fully exited
    /// - newState is fully entered
    ///
    /// INTERRUPTS:
    /// - SuperState → replaces PlayerController.CurrentMovementState
    /// - SubState → replaces current superstate's substate
    /// </summary>
    public void SwitchStates(PlayerBaseState newState, PlayerContext context, PlayerController ctx)
    {
        //current state exit
        ExitStates(context);

        //new state enter
        newState.EnterState(context);

        if (IsSuperState)
        {
            ctx.CurrentMovementState = newState;
        }
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState, context);
    }

    /// <summary>
    /// Updates this state and recursively updates active substates.
    /// </summary>
    public void UpdateStates(PlayerContext context)
    {
        UpdateState(context);

        if (_currentSubState != null)
            _currentSubState.UpdateStates(context);
    }

    /// <summary>
    /// Exits this state and recursively exits active substates.
    /// </summary>
    public void ExitStates(PlayerContext context)
    {
        ExitState(context);

        if (_currentSubState != null)
            _currentSubState.ExitStates(context);

        _currentSubState = null;
    }

    #endregion

    #region Protected Api

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    /// <summary>
    /// Assigns and enters a substate.<para/>
    ///
    /// ASSUMES:
    /// - Current state is already entered<para/>
    ///
    /// GUARANTEES:
    /// - Substate EnterState() has executed
    /// </summary>
    protected void SetSubState(PlayerBaseState newSubState, PlayerContext context)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        newSubState.EnterState(context);
    }

    #endregion
}