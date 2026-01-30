public class PlayerCameraStateFactory
{
    private PlayerController _ctx;
    public PlayerCameraStateFactory(PlayerController _currentContext) { _ctx = _currentContext; }

    //rootStates
    public PlayerCameraBaseState MainCamera()
    {
        PlayerCameraBaseState state = new PlayerMainCamera(_ctx, this);
        state.IsRootSate = true;
        return state;
    }
    public PlayerCameraBaseState AimCamera()
    {
        PlayerCameraBaseState state = new PlayerAimCamera(_ctx, this);
        state.IsRootSate = true;
        return state;
    }
}
