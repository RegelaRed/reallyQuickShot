using System.Collections.Generic;
using UnityEngine;

public class Ranged_Bow : WeaponsBase
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private List<ProjectileData> _ammoDataObject;

    private ProjectileData _currentAmmoDataObject;
    private int _currentAmmoIndex;

    private float _reloadTimer;
    private int _currentAmmo;
    private float _charge;

    private bool _idlePos = true;

    private bool IsReloading => _reloadTimer > 0f;
    private bool CanAttack => _currentAmmo > 0 && !IsReloading;
    private bool CanReload => _currentAmmo < _weaponData.maxAmmo;

    // ─────────────────────────────────────────────

    public override void UpdateWeapon(WeaponInput input)
    {
        HandleAttack(input);
        HandleReload(input);
        HandleAmmoSwitch(input);
        UpdateTimers();
    }

    // ───────────── Attack Flow ─────────────

    private void HandleAttack(WeaponInput input)
    {
        if (IsReloading) return;

        if (input.AttackHeld)
        {
            ChargeAttack();
            SetActivePose();
        }
        else if (input.AttackReleased)
        {
            ReleaseAttack();
            SetIdlePose();
        }
    }

    private void ChargeAttack()
    {
        if (!CanAttack) return;

        _charge += _weaponData.chargeRate * Time.deltaTime;
        _charge = Mathf.Min(_charge, _weaponData.maxCharge);
    }

    private void ReleaseAttack()
    {
        if (_charge <= 0f) return;
        if (!CanAttack) { _charge = 0f; return; }

        Attack();
        _charge = 0f;
    }

    public override void Attack()
    {
        _currentAmmo--;
        float chargePct = _charge / _weaponData.maxCharge;

        _wtx.Spawner.CreateProjectile(
            _currentAmmoDataObject,
            chargePct,
            _wtx.Controller.Orientation
        );
    }

    // ───────────── Reload ─────────────

    private void HandleReload(WeaponInput input)
    {
        if (input.ReloadPressed && CanReload && !IsReloading)
        {
            _reloadTimer = _weaponData.reloadTime;
            _charge = 0f;
        }
    }

    private void UpdateTimers()
    {
        if (_reloadTimer <= 0f) return;

        _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0f)
        {
            _currentAmmo = _weaponData.maxAmmo;
            _charge = 0f;
        }
    }

    // ───────────── Ammo ─────────────

    private void HandleAmmoSwitch(WeaponInput input)
    {
        if (input.AmmoNext) SwitchAmmo(1);
        else if (input.AmmoPrevious) SwitchAmmo(-1);
    }

    private void SwitchAmmo(int dir)
    {
        int count = _ammoDataObject.Count;
        int index = (_currentAmmoIndex + dir + count) % count;
        EquipAmmo(index);
    }

    private void EquipAmmo(int index)
    {
        _currentAmmoIndex = index;
        _currentAmmoDataObject = _ammoDataObject[index];
    }

    // ───────────── Visuals ─────────────

    private void SetActivePose()
    {
        if (_idlePos || _wtx.Input.IsAiming)
        {
            transform.localPosition = _wtx.WeaponPositionActive.position;
            _idlePos = false;
        }
    }

    private void SetIdlePose()
    {
        if (!_idlePos || !_wtx.Input.IsAiming)
        {
            transform.localPosition = _wtx.WeaponPositionIdle.position;
            _idlePos = true;
        }
    }

    // ───────────── Init ─────────────

    public override void OnInitialize(PlayerWeaponsManager wtx)
    {
        if (_ammoDataObject == null || _ammoDataObject.Count == 0)
        {
            Debug.LogError("No ammo assigned", this);
            enabled = false;
            return;
        }

        _currentAmmo = _weaponData.maxAmmo;
        EquipAmmo(0);
    }
}
