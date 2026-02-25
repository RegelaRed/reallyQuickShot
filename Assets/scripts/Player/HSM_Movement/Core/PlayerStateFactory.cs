using UnityEngine;

/// <summary>
/// Responsible for creating and configuring player state instances.<para/>
/// Provides factory methods for both root and sub states.
/// </summary>
public class PlayerStateFactory
{
    private PlayerMotor _playerMotor;
    public PlayerStateFactory(PlayerMotor playerMotor)
    {
        _playerMotor = playerMotor;
    }
    //Super States
    /// <summary>
    /// This is a SuperState
    /// </summary>
    /// <returns></returns>
    public PlayerBaseState Grounded() => new PlayerGroundedState(this, _playerMotor) { IsSuperState = true };
    public PlayerBaseState Falling() => new PlayerFallingState(this, _playerMotor) { IsSuperState = true };
    public PlayerBaseState Jump() => new PlayerJumpState(this, _playerMotor) { IsSuperState = true };
    public PlayerBaseState Dash() => new PlayerDashState(this, _playerMotor) { IsSuperState = true };

    //Sub States
    public PlayerBaseState Idle() => new PlayerIdleState(this, _playerMotor);
    public PlayerBaseState Walk() => new PlayerWalkState(this, _playerMotor);
    public PlayerBaseState Sprint() => new PlayerSprintState(this, _playerMotor);
}
