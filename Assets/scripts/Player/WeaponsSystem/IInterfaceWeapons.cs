public interface IWeapons
{
    public WeaponData _data { get; }
    public WeaponTypeEnum _type { get; }
    public void Reload();
    public void Fire();
    public void Equip();
    public void Unequip();

    public bool CanFire();
    public bool CanReload();
}