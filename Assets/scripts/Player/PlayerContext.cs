using UnityEngine;
public struct PlayerContext
{
    public PlayerInputSnapshot Input;
    public PlayerVariables Variables;
    public PlayerInputBuffer InputBuffer;

    public Vector3 Velocity;
    public bool IsGrounded;

    public float JumpCooldown;

    public float DashCooldown;
    public float DashRegenTimer;
    public int DashCharges;
}
public static class DashRules
{
    public static bool CanDash(in PlayerContext ctx) => ctx.DashCharges > 0 && ctx.DashCooldown <= 0f;
    public static void Consume(ref PlayerContext ctx, ref PlayerVariables var)
    {
        ctx.DashCharges--;
        ctx.DashCooldown = var.dashInterval;
        ctx.DashRegenTimer = var.dashRegenTime;
    }
}
public static class JumpRules
{
    public static bool CanJump(in PlayerContext ctx) => ctx.JumpCooldown <= 0f && ctx.IsGrounded;
    public static void Consume(ref PlayerContext ctx, ref PlayerVariables var)
    {
        ctx.JumpCooldown = var.jumpInterval;
    }
}
