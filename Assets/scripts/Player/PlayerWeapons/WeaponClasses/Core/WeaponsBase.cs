using UnityEngine;

public abstract class WeaponsBase : MonoBehaviour
{
    public PlayerWeaponsManager _wtx;
    public virtual void OnInitialize(PlayerWeaponsManager wtx)
    {
        _wtx = wtx;
    }
    public virtual void Equip(WeaponContext weaponContext) { }
    public virtual void UnEquip(WeaponContext weaponContext) { }
    public abstract void Attack(WeaponContext weaponContext);
    public abstract void UpdateWeapon(WeaponContext weaponContext);
}