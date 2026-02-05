using UnityEngine;
public abstract class PlayerCameraBaseState
{
    //priavte variables
    private bool _isRootState = false;
    private PlayerController _ctx;
    private PlayerCameraStateFactory _factory;
    private PlayerCameraBaseState _currentSuperState;
    private PlayerCameraBaseState _currentSubState;

    //camera settings
    private float _pitch;
    private float _yaw;
    private float _minPitch;
    private float _maxPitch;
    private float _sensX;
    private float _sensY;

    //Getter/Setters
    public bool IsRootState { get { return _isRootState; } set { _isRootState = value; } }
    public PlayerController Ctx { get { return _ctx; } }
    public PlayerCameraStateFactory Factory { get { return _factory; } }
    public PlayerCameraBaseState CurrentSuperState { get { return _currentSuperState; } }
    public PlayerCameraBaseState CurrentSubState { get { return _currentSubState; } }

    public float Pitch { get { return _pitch; } set { _pitch = value; } }
    public float Yaw { get { return _yaw; } set { _yaw = value; } }
    public float MinPitch { get { return _minPitch; } set { _minPitch = value; } }
    public float MaxPitch { get { return _maxPitch; } set { _maxPitch = value; } }

    //constructor
    public PlayerCameraBaseState(PlayerController ctx, PlayerCameraStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    //Default States
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void CheckSwitchState();
    protected void SwitchStates(PlayerCameraBaseState newState)
    {
        ExitState();
        newState.EnterState();

        if (IsRootState)
        {
            Ctx.CurrentCameraState = newState;
        }
        else if (_currentSubState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }
    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();
    }
    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
            _currentSubState.ExitStates();
    }
    public void SetSuperstate(PlayerCameraBaseState newState)
    {
        _currentSuperState = newState;
    }
    public void SetSubState(PlayerCameraBaseState newState)
    {
        _currentSubState = newState;
        _currentSubState.SetSuperstate(this);
        _currentSubState.EnterState();
    }

    //CameraSpecific Functions
    public void HandleRotation(Vector2 lookInput)
    {
        Yaw += lookInput.x * _sensX * Time.deltaTime;
        Pitch -= lookInput.y * _sensY * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch, MinPitch, MaxPitch);

        Ctx.Orientation.rotation = Quaternion.Euler(0f, Yaw, 0f);
        Ctx.PlayerCameraPosition.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
    }

    public void SetPitchLimits(float min, float max)
    {
        _minPitch = min;
        _maxPitch = max;
    }

    public void SetSensitivity(float sensX, float sensY)
    {
        _sensX = sensX;
        _sensY = sensY;
    }
}