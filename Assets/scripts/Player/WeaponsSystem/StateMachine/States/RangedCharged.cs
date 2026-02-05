using UnityEngine;

public class RangedCharged : AttackStatebase
{
    public RangedCharged(PlayerWeaponsManager ctxWeapons, AttackStateFactory factory) : base(ctxWeapons, factory)
    { }

    private float _charge = 0f;
    private bool last_state;
    public override void Enter() { }
    public override void Exit() { }
    public override void UpdateWeapon()
    {
        IWeapons weapon = CtxWeapons.CurrentWeapons;
        if (weapon.CanFire() && CtxWeapons.Input.AttackHeld)
        {
            last_state = true;
            _charge = Mathf.Min(_charge + (weapon.Data.chargeRate * Time.deltaTime), weapon.Data.maxCharge);
        }
        else if (weapon.CanFire() && !CtxWeapons.Input.AttackHeld && last_state)
        {
            last_state = false;

            weapon.Fire(_charge);
        }
    }
    public override void CheckSwitch() { }
}