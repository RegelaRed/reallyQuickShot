using UnityEngine;

/// <summary>
/// Aim-mode camera state. <para/>
/// Provides scoped camera look with potentially different sensitivity/pitch limits than main camera.
/// Exits when aim mode is toggled off.
/// </summary>
public class PlayerAimCamera : PlayerCameraBaseState
{
    /// <summary> Creates the AimMode camera state </summary>
    public PlayerAimCamera(PlayerController ctx, PlayerCameraStateFactory factory) : base(ctx, factory)
    { }

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
        return null;
    }

    /// <summary>
    /// Rotates the player body to face the movement direction.
    /// Rotation occurs only on the horizontal plane.
    /// </summary>
    private void FaceDirection()
    {
        Ctx.FaceDirection.rotation = Quaternion.LookRotation(Ctx.Orientation.forward);
    }
}
