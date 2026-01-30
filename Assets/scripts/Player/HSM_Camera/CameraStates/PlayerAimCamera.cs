using UnityEngine;
public class PlayerAimCamera : PlayerCameraBaseState
{
    public PlayerAimCamera(PlayerController _ctx, PlayerCameraStateFactory _factory)
    : base(_ctx, _factory)
    {
        SetSensitivity(Ctx.Variables.horizontalCameraSensitivity, Ctx.Variables.verticalCameraSensitivity);
        SetPitchLimits(Ctx.Variables.aimCameraPitchMin, Ctx.Variables.aimCameraPitchMax);
    }
    public override void EnterState() { Ctx.AimCamera.SetActive(true); }
    public override void ExitState() { Ctx.AimCamera.SetActive(false); }
    public override void UpdateState()
    {
        HandleRotation(Ctx.Input.CurrentLookInput);
        FaceDirection();
        CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (!Ctx.Input.IsAiming)
            SwitchStates(Factory.MainCamera());
    }
    private void FaceDirection()
    {
        Ctx.FaceDirection.rotation = Quaternion.LookRotation(Ctx.Orientation.forward);
    }
}
