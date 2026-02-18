using UnityEngine;
public class PlayerAimCamera : PlayerCameraBaseState
{
    public PlayerAimCamera(PlayerController _ctx, PlayerCameraStateFactory _factory)
    : base(_ctx, _factory)
    {

    }
    public override void EnterState(PlayerContext context)
    {
        SetSensitivity(context.Variables.horizontalCameraSensitivity, context.Variables.verticalCameraSensitivity);
        SetPitchLimits(context.Variables.aimCameraPitchMin, context.Variables.aimCameraPitchMax);

        Ctx.AimCamera.SetActive(true);
    }
    public override void ExitState(PlayerContext context) { Ctx.AimCamera.SetActive(false); }
    public override void UpdateState(PlayerContext context)
    {
        HandleRotation(context.Input.Look);
        FaceDirection();
        CheckSwitchState(context);
    }
    public override void CheckSwitchState(PlayerContext context)
    {
        if (!context.Input.AimMode)
            SwitchStates(Factory.MainCamera(), context);
    }
    private void FaceDirection()
    {
        Ctx.FaceDirection.rotation = Quaternion.LookRotation(Ctx.Orientation.forward);
    }
}
