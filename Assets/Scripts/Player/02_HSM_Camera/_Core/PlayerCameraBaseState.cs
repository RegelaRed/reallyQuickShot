using UnityEngine;

/// <summary>
/// Base class for all player camera states.
/// Implements a hierarchical state machine with support for root and sub-states,
/// and provides shared camera rotation logic.
/// </summary>
public abstract class PlayerCameraBaseState
{
    // Private variables
    private bool _isRootState = false;
    private PlayerController _ctx;
    private PlayerCameraStateFactory _factory;
    private PlayerMotor _playerMotor;
    private PlayerCameraBaseState _currentSuperState;
    private PlayerCameraBaseState _currentSubState;

    // Camera settings

    /// <summary>Vertical camera rotation in degrees (looking up/down).</summary>
    private float _pitch;

    /// <summary>Horizontal camera rotation in degrees (looking left/right).</summary>
    private float _yaw;

    /// <summary>Minimum allowed pitch angle.</summary>
    private float _minPitch;

    /// <summary>Maximum allowed pitch angle.</summary>
    private float _maxPitch;

    /// <summary>Horizontal mouse sensitivity multiplier.</summary>
    private float _sensX;

    /// <summary>Vertical mouse sensitivity multiplier.</summary>
    private float _sensY;

    // Getter/Setters

    /// <summary>
    /// True if this state is the root state of the camera state machine.
    /// Root states update the PlayerController's CurrentCameraState.
    /// </summary>
    public bool IsRootState { get => _isRootState; set => _isRootState = value; }

    /// <summary>Reference to PlayerController.</summary>
    public PlayerController Ctx => _ctx;

    /// <summary>Reference to state factory used to create camera states.</summary>
    public PlayerCameraStateFactory Factory => _factory;

    /// <summary>Reference to player motor (optional dependency).</summary>
    public PlayerMotor Motor => _playerMotor;

    /// <summary>Parent state in the hierarchy.</summary>
    public PlayerCameraBaseState CurrentSuperState => _currentSuperState;

    /// <summary>Child state in the hierarchy.</summary>
    public PlayerCameraBaseState CurrentSubState => _currentSubState;

    /// <summary>Camera vertical rotation in degrees.</summary>
    public float Pitch { get => _pitch; set => _pitch = value; }

    /// <summary>Camera horizontal rotation in degrees.</summary>
    public float Yaw { get => _yaw; set => _yaw = value; }

    /// <summary>Minimum allowed pitch rotation.</summary>
    public float MinPitch { get => _minPitch; set => _minPitch = value; }

    /// <summary>Maximum allowed pitch rotation.</summary>
    public float MaxPitch { get => _maxPitch; set => _maxPitch = value; }

    // Constructors

    /// <summary>
    /// Initializes a camera state with required context and factory.
    /// </summary>
    public PlayerCameraBaseState(PlayerController ctx, PlayerCameraStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    /// <summary>
    /// Initializes a camera state with context, factory and player motor.
    /// </summary>
    public PlayerCameraBaseState(PlayerController ctx, PlayerCameraStateFactory factory, PlayerMotor playerMotor)
    {
        _ctx = ctx;
        _factory = factory;
        _playerMotor = playerMotor;
    }

    // State Lifecycle

    /// <summary>
    /// Called when the state becomes active.
    /// Used for initialization and setup.
    /// </summary>
    public abstract void EnterState(PlayerContext context);

    /// <summary>
    /// Called when the state is exited.
    /// Used for cleanup.
    /// </summary>
    public abstract void ExitState(PlayerContext context);

    /// <summary>
    /// Called every frame while this state is active.
    /// </summary>
    public abstract void UpdateState(PlayerContext context);

    /// <summary>
    /// Determines whether a state transition should occur.
    /// Returns the new state or null if no change is needed.
    /// </summary>
    public abstract PlayerCameraBaseState CheckSwitchState(PlayerContext context);

    /// <summary>
    /// Switches from the current state to a new state.
    /// Transfers yaw and pitch so camera rotation is preserved.
    /// </summary>
    public void SwitchStates(PlayerCameraBaseState newState, PlayerContext context)
    {
        // Preserve camera rotation between states
        newState.Yaw = Yaw;
        newState.Pitch = Pitch;

        ExitStates(context);
        newState.EnterState(context);

        // Root states directly update controller state reference
        if (IsRootState)
        {
            Ctx.CurrentCameraState = newState;
        }
        // Sub-states update via their parent state
        else if (_currentSubState != null)
        {
            _currentSuperState.SetSubState(newState, context);
        }
    }

    /// <summary>
    /// Updates this state and all active substates recursively.
    /// </summary>
    public void UpdateStates(PlayerContext context)
    {
        UpdateState(context);

        if (_currentSubState != null)
            _currentSubState.UpdateStates(context);
    }

    /// <summary>
    /// Exits this state and all active substates recursively.
    /// </summary>
    public void ExitStates(PlayerContext context)
    {
        ExitState(context);

        if (_currentSubState != null)
            _currentSubState.ExitStates(context);
    }

    /// <summary>
    /// Assigns the parent state.
    /// </summary>
    public void SetSuperstate(PlayerCameraBaseState newState, PlayerContext context)
    {
        _currentSuperState = newState;
    }

    /// <summary>
    /// Assigns and enters a new substate.
    /// </summary>
    public void SetSubState(PlayerCameraBaseState newState, PlayerContext context)
    {
        _currentSubState = newState;

        _currentSubState.SetSuperstate(this, context);
        _currentSubState.EnterState(context);
    }

    // Camera Functions

    /// <summary>
    /// Applies mouse/controller look input to camera rotation.
    /// </summary>
    /// <param name="lookInput">
    /// X = horizontal look input  
    /// Y = vertical look input
    /// </param>
    public void HandleRotation(Vector2 lookInput)
    {
        // Horizontal rotation:
        // Δyaw = input * sensitivity * frameTime
        Yaw += lookInput.x * _sensX * Time.deltaTime;

        // Vertical rotation:
        // Negative because screen Y increases downward
        Pitch -= lookInput.y * _sensY * Time.deltaTime;

        // Clamp vertical rotation to prevent unnatural head tilt
        Pitch = Mathf.Clamp(Pitch, MinPitch, MaxPitch);

        // Apply horizontal rotation to player orientation
        // This controls movement direction
        Ctx.Orientation.rotation =
            Quaternion.Euler(0f, Yaw, 0f);

        // Apply vertical rotation to camera pivot
        // This controls where the player looks
        Ctx.PlayerCameraPosition.localRotation =
            Quaternion.Euler(Pitch, 0f, 0f);
    }

    /// <summary>
    /// Sets minimum and maximum allowed vertical camera rotation.
    /// </summary>
    public void SetPitchLimits(float min, float max)
    {
        _minPitch = min;
        _maxPitch = max;
    }

    /// <summary>
    /// Sets camera look sensitivity multipliers.
    /// </summary>
    public void SetSensitivity(float sensX, float sensY)
    {
        _sensX = sensX;
        _sensY = sensY;
    }
}