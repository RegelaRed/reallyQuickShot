
using System;

public abstract class PlayerBaseState
{
    protected PlayerController ctx;
    protected PlayerStateFactory playerStateFactory;
    public PlayerBaseState(PlayerController currentContext, PlayerStateFactory stateFactory)
    {
        this.ctx = currentContext;
        this.playerStateFactory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void Update();
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
        ctx.CurrentState = newState;
    }
    protected void UpdateStates() { }
    protected void SetSuperState() { }
    protected void SetSubState() { }
}
