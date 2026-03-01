using UnityEngine;

public abstract class WeaponsBase : MonoBehaviour
{

    /// <summary>method called when weapon is Activated/Equiped  </summary>
    /// <param name="weaponContext"> weaponContext to pass in needed data </param>
    public virtual void Equip(WeaponContext weaponContext)
    {
        transform.SetParent(weaponContext.WeaponPosActive, false);
    }
    ///<summary> method called when weapon is Deactivated/Unequiped </summary>
    /// <param name="weaponContext"> weaponContext to pass in needed data </param>
    public virtual void UnEquip(WeaponContext weaponContext)
    {
        transform.SetParent(weaponContext.WeaponPosIdle, false);
    }
    public abstract void Attack(WeaponContext weaponContext);
    public abstract void TickWeapon(WeaponContext weaponContext);
    protected abstract void Timers(WeaponContext weaponContext);
    public abstract void OnInitialize(WeaponContext weaponContext);
}