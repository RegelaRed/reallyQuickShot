public interface IWeapons
{
    public WeaponData Data { get; }
    public WeaponTypeEnum Type { get; }
    public void Reload();
    public void Fire(float charge);
    public void Equip();
    public void Unequip();

    public bool CanFire();
    public bool CanReload();
}