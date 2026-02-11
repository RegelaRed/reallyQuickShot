using UnityEngine;
//this is the movement rules file
public static class DashRules
{
    public static bool CanDash(PlayerContext context) => context.DashCharges > 0 && context.DashIntervalTimer <= 0f;
    public static void Consume(PlayerContext context)
    {
        context.DashCharges--;
        context.DashIntervalTimer = context.Variables.dashInterval;
        context.DashRegenTimer = context.Variables.dashRegenTime;
    }
    public static void Regenerate(PlayerContext context)
    {
        context.DashCharges++;
        context.DashCharges = Mathf.Min(
            context.DashCharges,
            context.Variables.maxDashCharges
        );
        if (context.DashCharges < context.Variables.maxDashCharges)
            context.DashRegenTimer = context.Variables.dashRegenTime;
    }
}
public static class JumpRules
{
    public static bool CanJump(PlayerContext context) => context.JumpIntervalTimer <= 0f && context.IsGrounded;
    public static void Consume(PlayerContext context)
    {
        context.JumpIntervalTimer = context.Variables.jumpInterval;
    }
}