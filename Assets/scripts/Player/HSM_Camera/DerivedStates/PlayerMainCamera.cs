using UnityEngine;
public class PlayerMainCamera : PlayerCameraBaseState
{
    public PlayerMainCamera(PlayerController _ctx, PlayerCameraStateFactory _factory)
    : base(_ctx, _factory)
    {

    }
    public override void EnterState(PlayerContext context)
    {
        SetSensitivity(context.Variables.horizontalCameraSensitivity, context.Variables.verticalCameraSensitivity);
        SetPitchLimits(context.Variables.mainCameraPitchMin, context.Variables.mainCameraPitchMax);
        Ctx.MainCamera.SetActive(true);
    }
    public override void ExitState(PlayerContext context) { Ctx.MainCamera.SetActive(false); }
    public override void UpdateState(PlayerContext context)
    {
        HandleRotation(context.Input.Look);
        FaceDirection(context);
        CheckSwitchState(context);
    }
    public override void CheckSwitchState(PlayerContext context)
    {
        if (context.Input.AimMode)
            SwitchStates(Factory.AimCamera(), context);
    }
    private void FaceDirection(PlayerContext context)
    {
        Vector3 moveDir = context.PlayerMotor.FinalMoveVector;
        moveDir.y = 0f;
        if (moveDir.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            Ctx.FaceDirection.rotation = Quaternion.Slerp(
                Ctx.FaceDirection.rotation,
                targetRot,
                context.Variables.playerBodyRotationSpeed
            );
        }
    }
}