public class Ranged : AttackStatebase
{
    public Ranged(PlayerWeaponsManager ctxWeapons, AttackStateFactory factory) : base(ctxWeapons, factory) { }
    public override void Enter()
    {
        //start equip anim of the weapon
    }
    public override void Exit()
    {
        //start unequip animation of weapon
    }
    public override void UpdateWeapon()
    {
        IWeapons weapon = CtxWeapons.CurrentWeapons;
        if (weapon == null) return;

        if (CtxWeapons.Input.AttackHeld && weapon.CanFire())
        {
            weapon.Fire(0f);
        }

        if (CtxWeapons.Input.ReloadPressed && weapon.CanReload())
            weapon.Reload();

        CheckSwitch();
    }
    public override void CheckSwitch()
    {
        if (!CtxWeapons.Input.IsAiming && !CtxWeapons.Input.AttackHeld)
            SwitchWeapon(Factory.Idle());
    }
}
