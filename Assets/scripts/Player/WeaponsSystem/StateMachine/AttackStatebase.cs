public abstract class AttackStatebase
{
    private PlayerWeaponsManager _ctx;
    private AttackStateFactory _factory;

    public PlayerWeaponsManager Ctx { get { return _ctx; } }
    public AttackStateFactory Factory { get { return _factory; } }

    public AttackStatebase(PlayerWeaponsManager ctx, AttackStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateWeapon();
    public void SwitchWeapon(AttackStatebase newWeapon)
    {
        _ctx.CurrentWeapon.Exit();
        _ctx.CurrentWeapon = newWeapon;
        newWeapon.Enter();

    }
    public abstract void CheckSwitch();

}