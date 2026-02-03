public class Melee : AttackStatebase
{
    public Melee(PlayerWeaponsManager ctx, AttackStateFactory factory) : base(ctx, factory) { }
    public override void CheckSwitch() { }
    public override void Exit() { }
    public override void Enter() { }
    public override void UpdateWeapon() { }
}
