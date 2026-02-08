using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class temp : WeaponsBase
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private List<GameObject> _ammoPrefabs;

    private GameObject _currentAmmoPrefab;
    private int _ammoPrefabIndex;
    private int _currentAmmoIndex;

    private float _reloadTimer;
    private bool _reloading { get { return _reloadTimer > 0f; } }

    private int _currentAmmo;
    private float _charge;

    public bool _canAttack { get { return _currentAmmo > 0 && !(_reloadTimer > 0f); } }
    public bool _canReload { get { return _currentAmmo < _weaponData.maxAmmo; } }

    public override void Equip() { }
    public override void UnEquip() { }
    public override void UpdateWeapon()
    {
        if (_wtx.Input.AttackHeld && !_reloading) AttackPressed();
        else if (!_wtx.Input.AttackHeld && !_reloading) AtackReleased();

        Timers();
        if (_wtx.Input.ReloadPressed && _canReload && !_reloading) Reload();

        if (_wtx.Input.AmmoNext) SwitchAmmo();
        else if (_wtx.Input.AmmoPrevious) SwitchAmmo(-1);
    }
    private void AttackPressed()
    {
        if (!_canAttack) return;
        _charge += _weaponData.chargeRate * Time.deltaTime;
        _charge = Mathf.Min(_charge, _weaponData.maxCharge);
    }
    private void AtackReleased()
    {
        if (_charge > 0 && _canAttack)
        {
            Attack();
            _charge = 0f;
        }
    }
    public override void Attack()
    {
        _currentAmmo--;
        _wtx.Spawner.CreateProjectile(_charge, _wtx.Controller.Orientation, _currentAmmoPrefab);
    }
    public void Reload()
    {
        _reloadTimer = _weaponData.reloadTime;
    }
    private void Timers()
    {
        if (_reloadTimer > 0)
        {
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0f)
                _currentAmmo = _weaponData.maxAmmo;
        }
    }
    public override void OnInitialize(PlayerWeaponsManager wtx)
    {
        _currentAmmo = _weaponData.maxAmmo;
        EquipAmmo(0);
    }

    void SwitchAmmo(int x = 1)
    {
        int count = _ammoPrefabs.Count;
        int index = (_currentAmmoIndex + x + count) % count;
        EquipAmmo(index);
    }
    void EquipAmmo(int index)
    {
        if (index < 0 || index >= _ammoPrefabs.Count) return;

        _currentAmmoPrefab = _ammoPrefabs[index];
        _currentAmmoIndex = index;
    }
}