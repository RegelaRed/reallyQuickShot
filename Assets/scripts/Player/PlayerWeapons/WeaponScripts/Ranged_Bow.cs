using System;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_Bow : WeaponsBase
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private List<ProjectileData> _ammoDataObject;

    private ProjectileData _currentAmmoDataObject;
    private int _currentAmmoIndex;

    private float _reloadTimer;
    private bool _reloading { get { return _reloadTimer > 0f; } }

    private int _currentAmmo;
    private float _charge;

    //flags
    private bool _attackHeldActive;
    private bool _attackPos;
    private bool _idlePos;

    public bool _canAttack { get { return _currentAmmo > 0 && !(_reloadTimer > 0f); } }
    public bool _canReload { get { return _currentAmmo < _weaponData.maxAmmo; } }

    public override void Equip()
    {
        transform.position = _wtx.WeaponPositionIdle.position;
        _idlePos = true;
        gameObject.SetActive(true);
        Debug.Log($"Bow Activated");
    }
    public override void UnEquip()
    {
        transform.position = _wtx.WeaponPositionIdle.position;
        gameObject.SetActive(false);
        Debug.Log($"Bow Deactivated");
    }
    public override void UpdateWeapon(WeaponInput input)
    {
        if (input.AttackHeld && !_reloading)
        {
            _attackHeldActive = true;
            AttackPressed();
            if (_idlePos || _wtx.Input.IsAiming)
            {
                transform.position = _wtx.WeaponPositionActive.position;
                _idlePos = false;
            }

        }
        else if (!input.AttackHeld && !_reloading && _attackHeldActive)
        {
            _attackHeldActive = false;
            AtackReleased();
            if (!_idlePos || !_wtx.Input.IsAiming)
            {
                transform.position = _wtx.WeaponPositionIdle.position;
                _idlePos = true;
            }
        }

        Timers();
        if (input.ReloadPressed && _canReload && !_reloading) Reload();

        if (input.AmmoNext) SwitchAmmo();
        else if (input.AmmoPrevious) SwitchAmmo(-1);
    }
    private void AttackPressed()
    {
        if (!_canAttack) return;
        // Debug.Log($"Attack Pressed Called");
        _charge += _weaponData.chargeRate * Time.deltaTime;
        _charge = Mathf.Min(_charge, _weaponData.maxCharge);
    }
    private void AtackReleased()
    {
        // Debug.Log($"Attack Released Called");
        if (_charge > 0 && _canAttack)
        {
            Attack();
            _charge = 0f;
        }
    }
    public override void Attack()
    {
        _currentAmmo--;
        float chargePercentage = _charge / _weaponData.maxCharge;
        _wtx.Spawner.CreateProjectile(_currentAmmoDataObject, chargePercentage, _wtx.Controller.Orientation);
    }
    public void Reload()
    {
        _reloadTimer = _weaponData.reloadTime;
        _charge = 0f;
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
        if (_ammoDataObject == null || _ammoDataObject.Count == 0)
        {
            Debug.LogError("No ammo prefabs assigned", this);
            enabled = false;
            return;
        }
        _currentAmmo = _weaponData.maxAmmo;
        EquipAmmo(0);
    }

    void SwitchAmmo(int x = 1)
    {
        int count = _ammoDataObject.Count;
        int index = (_currentAmmoIndex + x + count) % count;
        EquipAmmo(index);
    }
    void EquipAmmo(int index)
    {
        if (index < 0 || index >= _ammoDataObject.Count) return;

        _currentAmmoDataObject = _ammoDataObject[index];
        _currentAmmoIndex = index;
    }
}