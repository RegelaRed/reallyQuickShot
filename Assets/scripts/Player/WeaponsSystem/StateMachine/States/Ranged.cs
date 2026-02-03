public class Ranged : AttackStatebase
{
    public Ranged(PlayerWeaponsManager ctx, AttackStateFactory factory) : base(ctx, factory) { }

    private WeaponData data;
    public override void Enter()
    {
        // data = Ctx.CurrentWeapon.weaponData;
        //start equip anim of the weapon
    }
    public override void Exit()
    {
        //start unequip animation of weapon
    }
    public override void UpdateWeapon()
    {
        if (Ctx.Input.AttackHeld)
        {

        }
    }
    public override void CheckSwitch() { }
}
