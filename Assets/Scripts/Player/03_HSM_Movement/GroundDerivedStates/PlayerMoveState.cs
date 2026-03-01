/// <summary>
/// Ground movement substate for when the player is moving. <para/>
/// Handles walk/sprint speed and aim-mode speed reduction.
/// Transitions to Idle when input falls below threshold.
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    private bool _sprintState;
    public PlayerMoveState(PlayerStateFactory stateFactory, PlayerMotor playerMotor, bool sprintState)
    : base(stateFactory, playerMotor)
    { _sprintState = sprintState; }
    public override void EnterState(PlayerContext context) { }
    /// <summary>
    /// Stops movement when exiting (resets speed to prevent momentum carryover).
    /// </summary>
    public override void ExitState(PlayerContext context) { context.CurrentSpeed = 0f; }
    /// <summary>
    /// Applies speed based on sprint state and aim mode. <para/>
    /// Aiming while attacking reduces speed for tactical gameplay.
    /// </summary>
    public override void UpdateState(PlayerContext context)
    {
        // Reduce speed during aim+attack for tactical control
        float speed;
        if (context.Input.AttackHeld && context.Input.AimMode)
            speed = context.Variables.aimModeSpeed;
        else
            speed = _sprintState ? context.Variables.sprintSpeed : context.Variables.walkSpeed;

        context.CurrentSpeed = speed;
        Motor.SetGroundMovementDirection(context);
    }
    /// <summary>
    /// Transitions to Idle when input magnitude drops below threshold.
    /// </summary>
    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        if (context.Input.Move.magnitude <= 0.1f)
            return Factory.Idle();

        return null;
    }
    public override void InitializeSubState(PlayerContext context) { }
}