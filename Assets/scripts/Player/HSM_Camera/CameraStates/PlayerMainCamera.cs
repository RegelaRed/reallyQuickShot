using UnityEngine;
public class PlayerMainCamera : PlayerCameraBaseState
{
    public PlayerMainCamera(PlayerController _ctx, PlayerCameraStateFactory _factory)
    : base(_ctx, _factory)
    {
        SetSensitivity(Ctx.Variables.horizontalCameraSensitivity, Ctx.Variables.verticalCameraSensitivity);
        SetPitchLimits(Ctx.Variables.mainCameraPitchMin, Ctx.Variables.mainCameraPitchMax);
    }
    public override void EnterState() { Ctx.MainCamera.SetActive(true); }
    public override void ExitState() { Ctx.MainCamera.SetActive(false); }
    public override void UpdateState()
    {
        HandleRotation(Ctx.Input.CurrentLookInput);
        FaceDirection();
        CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (Ctx.Input.IsAiming)
            SwitchStates(Factory.AimCamera());
    }
    private void FaceDirection()
    {
        Vector3 moveDir = Ctx.PlayerMotor.FinalMoveVector;
        moveDir.y = 0f;
        if (moveDir.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            Ctx.FaceDirection.rotation = Quaternion.Slerp(
                Ctx.FaceDirection.rotation,
                targetRot,
                Ctx.Variables.playerBodyRotationSpeed
            );
        }
    }
}