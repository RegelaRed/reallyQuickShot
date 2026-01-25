using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory) { }
    public override void EnterState() { }
    public override void Tick() { }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }

    float gravity;
    float initialJumpVelocity;
    float maxJumpHeight;
    float maxJumpTime;

    void SetupJumpVaraiables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
    }
    void HandleJump() { }
}
