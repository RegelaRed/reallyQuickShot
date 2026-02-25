using UnityEngine;

public class PlayerContext
{
    public PlayerInputSnapshot Input;
    public PlayerInputBuffer InputBuffer;

    public PlayerVariables Variables;

    // -------- Motor Variables --------
    public float CurrentGravity;
    public float CurrentSpeed;

    // -------- Ground --------
    public bool IsGrounded;

    // -------- Jump -------- 
    public float InitialJumpVerticalVelocity;
    public float JumpIntervalTimer;
    public float JumpTimeToApex;
    public float JumpGravity;
    // -------- Dash --------
    public int DashCharges;
    public float InitialDashVerticalVelocity;
    public float InitialDashHorizontalVelocity;
    public float CanDashIntervalTimer;
    public float DashRegenTimer;
    public Vector3 DashDirection;
    public float DashGravity;

    // -------- Tiemrs --------
    public float DeltaTime;


    public void AbilityTimers()
    {
        if (JumpIntervalTimer > 0f)
            JumpIntervalTimer -= DeltaTime;

        if (CanDashIntervalTimer > 0f)
            CanDashIntervalTimer -= DeltaTime;

        if (DashRegenTimer > 0f)
        {
            DashRegenTimer -= DeltaTime;
            if (DashRegenTimer <= 0f)
                DashRules.Regenerate(this);
        }
    }
}