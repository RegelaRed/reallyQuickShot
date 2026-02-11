using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }
    public override void EnterState(PlayerContext context)
    {
        context.PlayerMotor.SetGroundMovementInput(Vector2.zero);
    }
    public override void UpdateState(PlayerContext context) { }
    public override void ExitState(PlayerContext context) { }
    public override void CheckSwitchState(PlayerContext context) { }
    public override void InitializeSubState(PlayerContext context) { }
}