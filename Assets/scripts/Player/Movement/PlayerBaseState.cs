
using System;

public abstract class PlayerBaseState
{
    protected PlayerController _ctx;
    protected PlayerStateFactory _factory;
    public PlayerBaseState(PlayerController currentContext, PlayerStateFactory stateFactory)
    {
        this._ctx = currentContext;
        this._factory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void Tick();
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
    protected void SetSuperState() { }
    protected void SetSubState() { }
}
