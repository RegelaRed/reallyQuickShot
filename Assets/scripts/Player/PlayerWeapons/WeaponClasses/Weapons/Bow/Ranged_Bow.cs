using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Bow weapon with charge-up mechanic and multiple ammo types.
/// Implements a hold-to-charge, release-to-fire pattern with reload management.
/// </summary>
/// <remarks>
/// Core Mechanics:<para/>
/// - Hold attack to charge arrow (up to max charge rate)<para/>
/// - Release to fire with power based on charge percentage<para/>
/// - Reload blocks all actions until timer completes<para/>
/// - Supports multiple ammo types with independent ammo counts<para/>
/// 
/// State Flow:<para/>
/// Idle → Charging (attack held) → Fire (attack released) → Idle
/// Any State → Reloading (reload pressed) → Idle (timer complete)
/// </remarks>
public class Ranged_Bow : WeaponsBase
{
    #region References/Variables
    [Header("Bow Components")]
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private List<ProjectileData> _ammoDataObject = new List<ProjectileData>();
    [SerializeField] private Transform ProjectileLaunchPoint;

    // ------------  ------------
    // private Spawner _spawner;
    private ProjectileData _currentAmmoData;

    // ------------  ------------
    private float _reloadTimer;
    private List<int> _ammoCounts;
    private int _currentAmmoIndex;
    private float _currentCharge;

    // ------------  ------------
    private bool IsReloading => _reloadTimer > 0f;
    private bool CanAttack => CurrentAmmoCount > 0f && !IsReloading;
    private bool CanReload => CurrentAmmoCount < _weaponData.maxAmmo;
    private int CurrentAmmoCount { get => _ammoCounts[_currentAmmoIndex]; set => _ammoCounts[_currentAmmoIndex] = value; }

    #endregion
    #region  Tick Updates

    public override void UpdateWeapon(WeaponContext weaponContext)
    {
        // Handle reload 
        if (IsReloading)
        {
            Timers(weaponContext);
            return;
        }

        // Handle charging
        if (weaponContext.AttackHeld && CanAttack)
        {
            _currentCharge += _weaponData.chargeRate * weaponContext.DeltaTime;
            _currentCharge = Mathf.Min(_currentCharge, _weaponData.maxCharge); ;
        }

        // Handle release
        else if (weaponContext.AttackReleased && _currentCharge >= 0)
        {
            if (!CanAttack)
            {
                Debug.Log($"No ammo Reload Weapon");
            }
            else
            {
                Debug.Log($"Weapon Fired {_currentCharge}");
                Attack(weaponContext);
                _currentCharge = 0f;
            }
        }

        // Handle reload
        if (weaponContext.ReloadPressed && CanReload)
            StartReload();

        if (weaponContext.AmmoSwitch) SwitchAmmo(1);

    }
    #endregion
    #region Weapon Behaviour


    /// <summary>
    /// Starts Reload state
    /// Sets reload timer to reload time from weaponData and resets current charge
    /// </summary>
    private void StartReload()
    {
        Debug.Log("StartReload");
        _reloadTimer = _weaponData.reloadTime;
        _currentCharge = 0f;
    }

    public override void Timers(WeaponContext weaponContext)
    {
        _reloadTimer -= weaponContext.DeltaTime;
        if (_reloadTimer <= 0f && CurrentAmmoCount < _weaponData.maxAmmo)
        {
            Debug.Log($"Relaod Complete");
            CurrentAmmoCount = _weaponData.maxAmmo;
        }
    }

    /// <summary>Decrement Ammo Count and call spawnrt</summary>
    /// <param name="weaponContext">weaponContest for Projectile Spawn Position</param>
    public override void Attack(WeaponContext weaponContext)
    {
        CurrentAmmoCount--;
        _ammoCounts[_currentAmmoIndex] = CurrentAmmoCount;

        float charge = _currentCharge / _weaponData.maxCharge;

        // Debug.Log($"Current Ammo count {CurrentAmmoCount}");
        // Debug.Log($"Current Charge {_currentCharge}");
        weaponContext.Spawner.CreateProjectile(_currentAmmoData, charge, ProjectileLaunchPoint);
    }

    /// <summary>will switch ammo to next or previous based on direction passed</summary>
    /// <param name="direction"> int direction 1/-1 </param>
    private void SwitchAmmo(int direction = 1)
    {
        if (_ammoCounts == null || _ammoDataObject == null)
            return;

        int count = _ammoDataObject.Count;

        _currentAmmoIndex = (_currentAmmoIndex + direction + count) % count;

        _currentAmmoData = _ammoDataObject[_currentAmmoIndex];
        CurrentAmmoCount = _ammoCounts[_currentAmmoIndex];
        EquipAmmo(_currentAmmoIndex);
    }

    /// <summary>Sets the current ammod data object form Ammo List</summary>
    /// <param name="index"> int index of ammo to be selected</param>
    private void EquipAmmo(int index)
    {
        _currentAmmoData = _ammoDataObject[index];
    }

    /// <summary>Called once when Weapon is created</summary>
    /// <param name="weaponContext"></param>
    public override void OnInitialize(WeaponContext weaponContext)
    {
        if (_ammoDataObject == null || _ammoDataObject.Count == 0)
        {
            // Debug.Log($"ammo data object or count is NULL, {_ammoDataObject}, {_ammoDataObject.Count}");
            return;
        }

        _ammoCounts = new List<int>(_ammoDataObject.Count);
        if (GetComponent<Spawner>() == null)
            this.AddComponent<Spawner>();
        weaponContext.Spawner = GetComponent<Spawner>();

        for (int i = 0; i < _ammoDataObject.Count; i++)
        {
            _ammoCounts.Add(_weaponData.maxAmmo);
        }
        CurrentAmmoCount = _weaponData.maxAmmo;
        EquipAmmo(0);
    }

    #endregion
}