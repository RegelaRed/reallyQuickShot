using UnityEditor.Experimental.RestService;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory) { }

    public override void EnterState() { }
    public override void Update() { }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
