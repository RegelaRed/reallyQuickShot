using TMPro;
using UnityEngine;

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
    public abstract void EnterState(ref PlayerContext context);
    public abstract void ExitState(ref PlayerContext context);
    public abstract void UpdateState(ref PlayerContext context);
    public abstract void CheckSwitchState(ref PlayerContext context);
    public abstract void InitializeSubState(ref PlayerContext context);
    protected void SwitchStates(PlayerBaseState newState, ref PlayerContext context)
    {
        //current state exit
        ExitState(ref context);

        //new state enter
        newState.EnterState(ref context);

        if (IsRootState)
        {
            //switch current state context
            _ctx.CurrentMovementState = newState;
        }
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState, ref context);
    }
    /// <summary>Update SubstatesStates if any</summary>
    public void UpdateStates(ref PlayerContext context)
    {
        UpdateState(ref context);
        if (_currentSubState != null)
            _currentSubState.UpdateStates(ref context);
    }
    /// <summary>Exit All SubStates if any</summary>
    public void ExitStates(ref PlayerContext context)
    {
        ExitState(ref context);
        if (_currentSubState != null)
            _currentSubState.ExitStates(ref context);
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState, ref PlayerContext context)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        newSubState.EnterState(ref context);
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