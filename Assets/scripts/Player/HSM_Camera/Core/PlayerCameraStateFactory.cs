public class PlayerCameraStateFactory
{
    private PlayerController _ctx;
    private PlayerMotor _playerMotor;
    public PlayerCameraStateFactory(PlayerController ctx, PlayerMotor playerMotor)
    {
        _ctx = ctx;
        _playerMotor = playerMotor;
    }
    //rootStates
    public PlayerCameraBaseState MainCamera() => new PlayerMainCamera(_ctx, this, _playerMotor)
    { IsRootState = true };

    public PlayerCameraBaseState AimCamera() => new PlayerAimCamera(_ctx, this)
    { IsRootState = true };
}
