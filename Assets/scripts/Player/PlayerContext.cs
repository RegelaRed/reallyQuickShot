using UnityEngine;

public class PlayerContext
{
    public PlayerInputSnapshot Input;
    public PlayerInputBuffer InputBuffer;

    public PlayerVariables Variables;
    public PlayerMotor PlayerMotor;


    // -------- Ground --------
    public bool IsGrounded;

    // -------- Jump -------- 
    public float InitialJumpVelocity;
    public float JumpGravity;
    public float JumpIntervalTimer;

    // -------- Dash --------
    public int DashCharges;
    public float InitialDashVelocity;
    public float DashGravity;
    public float DashIntervalTimer;
    public float DashRegenTimer;

    public Vector3 DashDirection;

    // -------- Tiemrs --------
    public float DeltaTime;
    public void AbilityTimers()
    {
        if (JumpIntervalTimer > 0f)
            JumpIntervalTimer -= DeltaTime;

        if (DashIntervalTimer > 0f)
            DashIntervalTimer -= DeltaTime;

        if (DashRegenTimer > 0f)
        {
            DashRegenTimer -= DeltaTime;
            if (DashRegenTimer <= 0f)
                DashRules.Regenerate(this);
        }
    }
}