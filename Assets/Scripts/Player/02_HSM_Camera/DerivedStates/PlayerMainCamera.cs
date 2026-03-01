using UnityEngine;

/// <summary>
/// Default gameplay camera state.
/// Handles normal player look rotation and body alignment with movement direction.
/// </summary>
public class PlayerMainCamera : PlayerCameraBaseState
{
    /// <summary>
    /// Creates the main gameplay camera state.
    /// </summary>
    public PlayerMainCamera(PlayerController ctx, PlayerCameraStateFactory factory, PlayerMotor playerMotor)
    : base(ctx, factory, playerMotor) { }

    public override void EnterState(PlayerContext context)
    {
        SetSensitivity(
            context.Variables.horizontalCameraSensitivity,
            context.Variables.verticalCameraSensitivity);

        SetPitchLimits(
            context.Variables.mainCameraPitchMin,
            context.Variables.mainCameraPitchMax);

        Ctx.MainCamera.SetActive(true);
    }
    public override void ExitState(PlayerContext context)
    {
        Ctx.MainCamera.SetActive(false);
    }

    /// <summary>
    /// Updates camera rotation and player facing direction.
    /// Also checks for camera state transitions.
    /// </summary>
    public override void UpdateState(PlayerContext context)
    {
        // Apply look input to camera rotation
        HandleRotation(context.Input.Look);

        // Rotate player body toward movement direction
        FaceDirection(context);

        // Check if state switch is required
        CheckSwitchState(context);
    }

    public override PlayerCameraBaseState CheckSwitchState(PlayerContext context)
    {
        if (context.Input.AimMode)
            return Factory.AimCamera();

        return null;
    }


    /// <summary>
    /// Rotates the player body to face the movement direction.
    /// Rotation occurs only on the horizontal plane.
    /// </summary>
    private void FaceDirection(PlayerContext context)
    {
        // Final movement direction calculated by PlayerMotor
        Vector3 moveDir = Motor.CurrentMoveDirection;

        // Ignore vertical movement (we only rotate on Y axis)
        moveDir.y = 0f;

        // Avoid rotation when movement is nearly zero.
        // sqrMagnitude is used instead of magnitude to avoid sqrt cost.
        if (moveDir.sqrMagnitude >= 0.1f)
        {
            /*
             Smoothly rotate player body toward movement direction.

             Quaternion.LookRotation(moveDir)
                 → Target rotation facing movement direction

             Quaternion.Slerp(current, target, t)
                 → Smooth interpolation between rotations

             t = rotationSpeed * deltaTime
                 → Frame-rate independent rotation speed
            */
            Ctx.FaceDirection.rotation = Quaternion.Slerp(
                Ctx.FaceDirection.rotation,
                Quaternion.LookRotation(moveDir),
                context.Variables.playerBodyRotationSpeed * context.DeltaTime
            );
        }
    }
}