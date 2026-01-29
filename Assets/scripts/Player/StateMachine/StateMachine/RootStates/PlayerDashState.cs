using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController _ctx, PlayerStateFactory _factory)
    : base(_ctx, _factory) { }

    private Coroutine _dashRoutine;

    public override void EnterState()
    {
        Ctx.PlayerMotor.SetInpulse(Ctx.Variables.dashForce * Ctx.Orientation.forward.normalized);
        _dashRoutine = Ctx.StartCoroutine(DashTimer());
    }
    public override void UpdateState() { }
    public override void ExitState()
    {
        if (_dashRoutine != null)
        {
            Ctx.StopCoroutine(_dashRoutine);
        }
        Ctx.PlayerMotor.SetInpulse(Vector3.zero);
    }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (Ctx.IsOnGround)
            SwitchStates(Factory.Grounded());
        else
            SwitchStates(Factory.Falling());
    }
}
