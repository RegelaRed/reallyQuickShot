using UnityEngine;
public class Bow : MonoBehaviour, IWeapons
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private WeaponTypeEnum _weaponTypeEnum;
    public WeaponData _data { get { return _weaponData; } }
    public WeaponTypeEnum _type { get { return _weaponTypeEnum; } }

    public void Reload() { }
    public void Fire() { }
    public void Equip() { }
    public void Unequip() { }

    public bool CanFire() { throw new System.NotImplementedException(); }
    public bool CanReload() { throw new System.NotImplementedException(); }
}
