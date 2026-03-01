public class PlayerCameraStateFactory
{
    private PlayerMainCamera _mainCamera;
    private PlayerAimCamera _aimCamera;

    public PlayerCameraStateFactory(PlayerController ctx, PlayerMotor playerMotor)
    {
        _mainCamera = new PlayerMainCamera(ctx, this, playerMotor) { IsRootState = true };
        _aimCamera = new PlayerAimCamera(ctx, this) { IsRootState = true };
    }
    //rootStates
    public PlayerCameraBaseState MainCamera() => _mainCamera;
    public PlayerCameraBaseState AimCamera() => _aimCamera;
}
