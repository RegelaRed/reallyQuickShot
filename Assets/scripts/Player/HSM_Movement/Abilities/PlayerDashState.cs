public class PlayerDashState : PlayerBaseState
{

    /// Dash lifecycle 
    /// start dash -> consume dash charge, set dash duration timer to dash duration from variables
    /// set dash initial vertical and horizontal velocity to player motor
    /// check for exit conditions
    /// 
    /// dash regen and can dash interval timers will run in player controller



    private bool _isDashing;
    private bool _dashLeftGround;
    private float _dashDurationTimer;

    public PlayerDashState(PlayerStateFactory stateFactory, PlayerMotor playerMotor) :
     base(stateFactory, playerMotor)
    { }

    public override void EnterState(PlayerContext context)
    {
        context.CurrentGravity = context.DashGravity;
        StartDash(context);
        Motor.SetUpwardVelocity(context.InitialDashVerticalVelocity);
    }
    public override void ExitState(PlayerContext context)
    {
        context.InputBuffer.ConsumeJump();
        context.CurrentGravity = context.JumpGravity;
    }
    public override void UpdateState(PlayerContext context) { }
    public override PlayerBaseState CheckSwitchState(PlayerContext context)
    {
        DashStates(context);
        if (_isDashing)
            return this;

        if (context.IsGrounded)
            return Factory.Grounded();
        else
            return Factory.Falling();
    }
    public override void InitializeSubState(PlayerContext context) { }

    /// <summary>
    /// Starts the Dash Physics lifecycle
    /// Defined in PlayerMotor for simpler management of dash lifecycle  
    /// </summary>
    /// <param name="distance"> Distance the Dash Covers, used for Calculating Speed </param>
    /// <param name="duration"> Duration of Dash, Used for Calculating Speed and Dash State Lifetime </param>
    /// <param name="direction"> Direction of Dash </param>
    public void StartDash(PlayerContext context)
    {
        _isDashing = true;
        _dashLeftGround = false;
        _dashDurationTimer = context.Variables.dashDuration;

        context.CanDashIntervalTimer = context.Variables.dashInterval;
        context.CurrentSpeed = context.Variables.dashDistance / context.Variables.dashDuration;
        Motor.HorizontalVelocityVector = context.DashDirection;
    }
    /// <summary>
    /// Checks to reset dash related Variables and early Dash Exit
    /// </summary>
    /// <param name="context"> Player Context for GroundChecks </param>
    private void DashStates(PlayerContext context)
    {
        if (!_isDashing)
            return;

        if (!context.IsGrounded)
            _dashLeftGround = true;

        if (_dashLeftGround && context.IsGrounded)
            EndDash(context);

        if (_dashDurationTimer > 0f)
        {
            _dashDurationTimer -= context.DeltaTime;
            if (_dashDurationTimer <= 0f)
                EndDash(context);
        }
    }
    /// <summary>
    /// Reset dash related Varibles
    /// </summary>
    private void EndDash(PlayerContext context)
    {
        _isDashing = false;
        _dashDurationTimer = 0f;
        Motor.VerticalVelocity = 0f;
    }
}
