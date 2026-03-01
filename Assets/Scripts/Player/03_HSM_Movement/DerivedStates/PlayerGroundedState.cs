/// <summary>
/// Root grounded movement state.
/// Handles player behavior while on the ground and manages
/// movement substates (Idle, Walk, Sprint).
/// Responsible for transitioning to Jump, Dash, or Falling.
/// </summary>
public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    private bool _wasGroundedLastFrame;

    /// <summary>
    /// Initializes grounded movement values and selects the correct
    /// movement substate (Idle, Walk, or Sprint).
    /// </summary>
    public override void EnterState(PlayerContext context)
    {
        context.CurrentGravity = context.JumpGravity;
        context.CurrentSpeed = context.Variables.walkSpeed;

        InitializeSubState(context);
    }

    /// <summary>
    /// Handles exit behavior when leaving the grounded state.
    /// Applies coyote time if the player walks off a ledge.
    /// </summary>
    public override void ExitState(PlayerContext context)
    {
        if (_wasGroundedLastFrame && !context.IsGrounded)
            context.InputBuffer.SetCyoteTime(context.Variables.jumpBufferTime);
    }

    /// <summary>
    /// Updates grounded substates every frame.
    /// </summary>
    public override void UpdateState(PlayerContext context)
    {
        _wasGroundedLastFrame = context.IsGrounded;
        UpdateSubstate(context);
    }

    /// <summary>
    /// Determines if the player should transition to another
    /// root movement state.
    /// Priority:
    /// 1. Jump
    /// 2. Dash
    /// 3. Falling
    /// </summary>
    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        // ------------ Jump ------------
        if (context.InputBuffer.JumpBufferActive && context.IsGrounded)
        {
            context.InputBuffer.ConsumeJump();
            return Factory.Jump();
        }
        if (context.Input.JumpHeld && context.IsGrounded)
        {
            context.InputBuffer.SetCyoteTime(context.Variables.jumpBufferTime);
            return Factory.Jump();
        }

        // ------------ Dash ------------
        else if (context.InputBuffer.DashBufferActive && DashRules.CanDash(context))
        {
            DashRules.Consume(context);
            return Factory.Dash();
        }
        // ------------ Falling ------------
        else if (!context.IsGrounded)
            return Factory.Falling();

        return null;
    }

    /// <summary>
    /// Updates movement substate based on current input.
    /// Switches between Idle, Walk, and Sprint when needed.
    /// </summary>
    private void UpdateSubstate(PlayerContext context)
    {
        PlayerBaseState desiredState = GetDesiredState(context);

        if (CurrentSubState != desiredState)
            SetSubState(desiredState, context);
    }

    /// <summary>
    /// Selects the initial movement substate when entering grounded state.
    /// </summary>
    public override void InitializeSubState(PlayerContext context)
    {
        SetSubState(GetDesiredState(context), context);
    }

    /// <summary>
    /// Determines which movement substate should be active
    /// based on movement and sprint input.
    /// </summary>
    private PlayerBaseState GetDesiredState(PlayerContext context)
    {
        if (context.Input.MovePressed && context.Input.SprintToggle)
            return Factory.Sprint();
        else if (context.Input.MovePressed)
            return Factory.Walk();
        else
            return Factory.Idle();
    }
}