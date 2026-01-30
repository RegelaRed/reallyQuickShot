using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerController _ctx, PlayerStateFactory _factory)
     : base(_ctx, _factory) { }
    public override void EnterState()
    {
        Ctx.PlayerMotor.SetGravity(Ctx.Variables.gravity);
    }
    public override void UpdateState()
    {
        Ctx.PlayerMotor.SetAirMovementInput(Ctx.Input.CurrentMovementInput);
        CheckSwitchState();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        Ctx.TimeLeftOnGround += Time.deltaTime;
        if (Ctx.IsOnGround)
            SwitchStates(Factory.Grounded());
        else if (Ctx.Input.IsJumpPressed && Ctx.CyoteTrue)
            SwitchStates(Factory.Jump());
    }
    public override void InitializeSubState() { }
}