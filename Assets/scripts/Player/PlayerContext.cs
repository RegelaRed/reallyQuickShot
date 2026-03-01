/// <summary>
/// Runtime data container shared across player systems.
/// Holds input, ability state, timers and movement variables.
/// </summary>
public class PlayerContext
{
    // -------- Core --------

    public PlayerInputSnapshot Input;
    public PlayerInputBuffer InputBuffer;
    public PlayerVariables Variables;

    public float DeltaTime;

    // -------- Motor --------

    public float CurrentGravity;
    public float CurrentSpeed;

    public bool IsGrounded;

    // -------- Jump --------

    public bool IsJumping;
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
    public float DashGravity;

    /// <summary>
    /// Updates all ability cooldown timers.
    /// </summary>
    public void UpdateAbilityTimers()
    {
        if (JumpIntervalTimer > 0f)
            JumpIntervalTimer -= DeltaTime;

        if (CanDashIntervalTimer > 0f)
            CanDashIntervalTimer -= DeltaTime;

        if (DashRegenTimer > 0f)
        {
            DashRegenTimer -= DeltaTime;

            if (DashRegenTimer <= 0f)
            {
                DashRegenTimer = 0f;
                DashRules.Regenerate(this);
            }
        }
    }
}