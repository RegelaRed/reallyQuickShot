public abstract class AttackStatebase
{
    private PlayerWeaponsManager _ctxWeapons;
    private AttackStateFactory _factory;

    public PlayerWeaponsManager CtxWeapons { get { return _ctxWeapons; } }
    public AttackStateFactory Factory { get { return _factory; } }

    public AttackStatebase(PlayerWeaponsManager ctxWeapons, AttackStateFactory factory)
    {
        _ctxWeapons = ctxWeapons;
        _factory = factory;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateWeapon();
    public void SwitchWeapon(AttackStatebase newWeapon)
    {
        _ctxWeapons.CurrentAttackState.Exit();
        _ctxWeapons.CurrentAttackState = newWeapon;
        newWeapon.Enter();

    }
    public abstract void CheckSwitch();

}