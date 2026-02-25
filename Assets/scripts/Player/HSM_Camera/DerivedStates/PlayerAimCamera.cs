using UnityEngine;
public class PlayerAimCamera : PlayerCameraBaseState
{
    public PlayerAimCamera(PlayerController ctx, PlayerCameraStateFactory factory) : base(ctx, factory)
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
    public override PlayerCameraBaseState CheckSwitchState(PlayerContext context)
    {
        if (!context.Input.AimMode)
            return Factory.MainCamera();
        return this;
    }
    private void FaceDirection()
    {
        Ctx.FaceDirection.rotation = Quaternion.LookRotation(Ctx.Orientation.forward);
    }
}
