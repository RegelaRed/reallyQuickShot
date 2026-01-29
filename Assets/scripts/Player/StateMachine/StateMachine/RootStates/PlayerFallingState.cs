using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(PlayerController _ctx, PlayerStateFactory _factory)
     : base(_ctx, _factory) { }
    public override void EnterState()
    {
        Ctx.PlayerMotor.SetGravity(Ctx.Variables.gravity);
        Ctx.PlayerMotor.SetSpeed(Ctx.Variables.airMoveSpeed);
    }
    public override void UpdateState() { CheckSwitchState(); }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        Ctx.TimeLeftOnGround += Time.deltaTime;
        if (Ctx.IsOnGround)
            SwitchStates(Factory.Grounded());
        if (Ctx.Input.IsJumpPressed && Ctx.TimeLeftOnGround <= Ctx.Variables.cyoteTime)
            SwitchStates(Factory.Jump());
    }
    public override void InitializeSubState()
    { }
}