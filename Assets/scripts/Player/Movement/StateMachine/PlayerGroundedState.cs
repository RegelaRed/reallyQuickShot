using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { InitializeSubState(); }
    public override void EnterState() { }
    public override void UpdateState()
    {
        CheckSwitchState();

        _ctx.PlayerMotor.SetHorizontalVelocity(_ctx.Input.CurrentMovementInput * Time.deltaTime);
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (_ctx.Input.IsJumpPressed)
        {
            SwitchStates(_factory.Jump());
        }
        else if (_ctx.Input.IsDashPressed)
        {
            SwitchStates(_factory.Dash());
        }
    }
    public override void InitializeSubState()
    {
        if (!_ctx.Input.IsMovementPressed && !_ctx.Input.IsSprintPressed)
        {
            SwitchStates(_factory.Idle());
        }
        else if (_ctx.Input.IsMovementPressed && !_ctx.Input.IsSprintPressed)
        {
            SwitchStates(_factory.Walk());
        }
        else if (_ctx.Input.IsMovementPressed && _ctx.Input.IsSprintPressed)
        {
            SwitchStates(_factory.Sprint());
        }
    }
}
