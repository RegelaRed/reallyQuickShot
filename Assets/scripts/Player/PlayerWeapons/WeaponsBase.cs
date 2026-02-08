using UnityEngine;

public abstract class WeaponsBase : MonoBehaviour
{
    public PlayerWeaponsManager _wtx;
    public virtual void OnInitialize(PlayerWeaponsManager wtx)
    {
        _wtx = wtx;
    }
    public virtual void Equip() { }
    public virtual void UnEquip() { }
    public abstract void Attack();
    public abstract void UpdateWeapon(WeaponInput input);
}