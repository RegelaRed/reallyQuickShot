using UnityEngine;

/// <summary>
/// Ground movement substate for stationary player. <para/>
/// Stops horizontal movement when player has no input.
/// Transitions to Move when input is detected.
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateFactory stateFactory, PlayerMotor playerMotor)
    : base(stateFactory, playerMotor)
    { }

    /// <summary>
    /// Zeros horizontal movement direction.
    /// </summary>
    public override void EnterState(PlayerContext context) { Motor.SetHorizontalDirectionVector(Vector3.zero); }
    public override void ExitState(PlayerContext context) { }
    public override void UpdateState(PlayerContext context) { }
    public override PlayerBaseState CheckSwitchState(PlayerContext context) { return null; }
    public override void InitializeSubState(PlayerContext context) { }
}