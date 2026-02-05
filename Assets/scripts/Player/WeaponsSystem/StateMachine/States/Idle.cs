public class Idle : AttackStatebase
{
    public Idle(PlayerWeaponsManager ctxWeapons, AttackStateFactory factory) : base(ctxWeapons, factory) { }
    public override void CheckSwitch() { }
    public override void Exit() { }
    public override void Enter() { }
    public override void UpdateWeapon() { }
}
