using UnityEngine;
public class Bow : MonoBehaviour, IWeapons
{
    [SerializeField] private WeaponData _weaponData;
    public WeaponData Data => _weaponData;
    public WeaponTypeEnum Type => _weaponData.type;
    private float _lastFireTime;
    private int _currentAmmo;

    private void Start()
    {
        _currentAmmo = _weaponData.ammoCapacity;
    }

    public void Reload()
    {
        if (!CanReload()) return;

        _currentAmmo = _weaponData.ammoCapacity;
        Debug.Log($"Reolad Complete, current ammo {_currentAmmo}");
    }
    public void Fire(float Charge)
    {
        if (!CanFire()) return;
        _lastFireTime = Time.time;
        _currentAmmo--;
        Debug.Log($"Arrow fired, remaining ammo {_currentAmmo}");
    }
    public void Equip() { gameObject.SetActive(true); }
    public void Unequip() { gameObject.SetActive(false); }

    public bool CanFire()
    {
        return _currentAmmo > 0 && Time.time >= _lastFireTime + _weaponData.firerate;
    }
    public bool CanReload() { return _currentAmmo < _weaponData.ammoCapacity; }
}
