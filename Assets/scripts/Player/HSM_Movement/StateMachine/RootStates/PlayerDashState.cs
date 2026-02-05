using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController _ctx, PlayerStateFactory _factory) : base(_ctx, _factory)
    { }

    public override void EnterState()
    {
        if (CurrentSubState != null)
            CurrentSubState.ExitStates();
        
        Vector3 dashDirection;
        if (Ctx.CurrentCameraState is PlayerAimCamera)
            dashDirection = Ctx.Orientation.forward;
        else
            dashDirection = Ctx.FaceDirection.forward;

        Ctx.PlayerMotor.SetGravity(Ctx.DashGravity);
        Ctx.PlayerMotor.StartDash(Ctx.Variables.dashDistance, Ctx.Variables.dashDuration, dashDirection);
        Ctx.PlayerMotor.SetUpwardVelocity(Ctx.InitialDashVelocity);
    }
    public override void ExitState() { }
    public override void UpdateState() { CheckSwitchState(); }
    public override void CheckSwitchState()
    {
        if (Ctx.PlayerMotor.IsDashing)
            return;

        if (Ctx.IsOnGround)
            SwitchStates(Factory.Grounded());
        else
            SwitchStates(Factory.Falling());
    }
    public override void InitializeSubState() { }

    public IEnumerator StartDash()
    {
        yield return new WaitForSeconds(Ctx.Variables.dashDuration);
        ExitState();
    }
}
