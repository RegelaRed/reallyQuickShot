using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera
{
    private readonly PlayerController ctx;
    public PlayerCamera(PlayerController ctx)
    {
        this.ctx = ctx;
    }

    public float rotationSpeed = 3f;

    public void LockPlayerCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockPlayerCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Tick()
    {
        PlayerCameraRotation();
        RotateTowardsMoveDirection(ctx.Input.MoveDirection);
        SwitchCamera();
    }

    private void PlayerCameraRotation()
    {
        if (ctx.PlayerCamera != null) ctx.PlayerCamera.rotation = Quaternion.Euler(ctx.Input.xRotation, ctx.Input.yRotation, 0f);

        ctx.Orientation.rotation = Quaternion.Euler(0f, ctx.Input.yRotation, 0f);
    }

    private void RotateTowardsMoveDirection(Vector3 moveDir)
    {
        // Flatten direction
        moveDir.y = 0f;
        moveDir.Normalize();

        Quaternion targetRotation;

        if (ctx.Input.AimModeActive)
            targetRotation = Quaternion.LookRotation(ctx.Orientation.forward);
        else
            targetRotation = Quaternion.LookRotation(moveDir);

        if (ctx.FaceDirection.rotation == targetRotation)
            return;

        ctx.FaceDirection.rotation = Quaternion.RotateTowards(
                ctx.FaceDirection.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime * 360f
            );
    }

    private void SwitchCamera()
    {
        if (ctx.Input.AimModeActive && !ctx.AimCamera.activeInHierarchy)
        {
            ctx.MainCamera.SetActive(false);
            ctx.AimCamera.SetActive(true);

            //corutine to enable aim reticle(wil add this lalter)
        }
        else if (!ctx.Input.AimModeActive && !ctx.MainCamera.activeInHierarchy)
        {
            ctx.AimCamera.SetActive(false);
            ctx.MainCamera.SetActive(true);
        }
    }
}