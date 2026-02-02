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

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();
    protected void SwitchStates(PlayerBaseState newState)
    {
        //current state exit
        ExitState();

        //new state enter
        newState.EnterState();

        if (IsRootState)
        {
            //switch current state context
            _ctx.CurrentMovementState = newState;
        }
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);

    }
    /// <summary>Update SubstatesStates if any</summary>
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();

        Debug.Log($"Superstate = {_currentSuperState}, Substate = {_currentSubState}");
    }
    /// <summary>Exit All SubStates if any</summary>
    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
            _currentSubState.ExitStates();
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
        newSubState.EnterState();
    }
}
///Template
/// public *StateNeme* (PlayerController _ctx, PlayerStateFactory _factory)
/// : base(_ctx, _factory) { }
/// public override void EnterState() { }
/// public override void UpdateState() { }
/// public override void ExitState() { }
/// public override void CheckSwitchState() { }
/// public override void InitializeSubState() { }