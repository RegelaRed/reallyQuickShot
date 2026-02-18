using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Ranged_Bow : WeaponsBase
{
    #region References/Variables
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private List<ProjectileData> _ammoDataObject = new List<ProjectileData>();

    Spawner _spawner;
    private ProjectileData _currentAmmoData;
    private int _currentAmmoIndex;

    private float _reloadTimer;
    private List<int> _ammoCounts = new List<int>();
    private int _currentAmmoCount;
    private float _currentCharge;

    private bool _idlePos = true;

    private bool IsReloading => _reloadTimer > 0f;
    private bool CanAttack => _currentAmmoCount > 0f && !IsReloading;
    private bool CanReload => _currentAmmoCount < _weaponData.maxAmmo;
    #endregion
    #region  Tick Updates
    /// <summary>
    /// Update Weapon Behaviour
    /// </summary>
    /// <param name="weaponContext"></param>
    public override void UpdateWeapon(WeaponContext weaponContext)
    {
        // Handle reload 
        if (IsReloading)
        {
            TickReload(weaponContext.DeltaTime);
            return;
        }

        // Handle charging
        if (weaponContext.AttackHeld && CanAttack)
        {
            _currentCharge += _weaponData.chargeRate * weaponContext.DeltaTime;
            _currentCharge = Mathf.Min(_currentCharge, _weaponData.maxCharge);
            Debug.Log($"Weapon Charging {_currentCharge}");
        }

        // Handle release
        if (weaponContext.AttackReleased && _currentCharge >= 0)
        {
            Debug.Log($"Weapon Fired {_currentCharge}");
            Attack(weaponContext);
            _currentCharge = 0f;
        }

        // Handle reload
        if (weaponContext.ReloadPressed && CanReload)
            StartReload();

        if (weaponContext.AmmoNext) SwitchAmmo(1);
        if (weaponContext.AmmoPrevious) SwitchAmmo(-1);
    }

    /// <summary>
    /// Set ReloadTimer and reset accumulates charge to 0
    /// </summary>
    private void StartReload()
    {
        _reloadTimer = _weaponData.reloadTime;
        _currentCharge = 0f;
    }
    /// <summary>
    /// tick down reload timer
    /// </summary>
    /// <param name="DeltaTime">weaponContest for DeltaTime</param>
    private void TickReload(float DeltaTime)
    {
        _reloadTimer -= DeltaTime;
    }
    /// <summary>
    /// Decrement Ammo Count and call spawnrt 
    /// </summary>
    /// <param name="weaponContext">weaponContest for Projectile Spawn Position</param>
    public override void Attack(WeaponContext weaponContext)
    {
        _currentAmmoCount--;
        _spawner.CreateProjectile(_currentAmmoData, _currentCharge, weaponContext.WeaponPosActive);
    }
    #endregion
    #region Weapon Behaviour
    /// <summary>
    /// will switch ammo to next or previous based on direction passed
    /// </summary>
    /// <param name="direction"> int direction 1/-1 </param>
    private void SwitchAmmo(int direction = 1)
    {
        int count = _ammoDataObject.Count;
        _currentAmmoIndex = (_currentAmmoIndex + direction + count) % count;

        _currentAmmoData = _ammoDataObject[_currentAmmoIndex];
        _currentAmmoCount = _ammoCounts[_currentAmmoIndex];
        EquipAmmo(_currentAmmoIndex);
    }
    /// <summary>
    /// Sets the current ammod data object form Ammo List
    /// </summary>
    /// <param name="index"> int index of ammo to be selected</param>
    private void EquipAmmo(int index)
    {
        _currentAmmoData = _ammoDataObject[index];
        Debug.Log($"Ammo index: {index} , AmmoData: {_currentAmmoData}");
    }
    /// <summary>
    /// method called when weapon is Activated/Equiped
    /// </summary>
    /// <param name="weaponContext"> weaponContext to pass in needed data </param>
    public override void Equip(WeaponContext weaponContext)
    {
        SetActivePose(weaponContext);
    }
    /// <summary>
    /// method called when weapon is Deactivated/Unequiped
    /// </summary>
    /// <param name="weaponContext"> weaponContext to pass in needed data </param>
    public override void UnEquip(WeaponContext weaponContext)
    {
        SetIdlePose(weaponContext);
    }
    /// <summary>
    /// Set position to active position object
    /// </summary>
    /// <param name="weaponContext">weaponContext for Position</param>
    private void SetActivePose(WeaponContext weaponContext)
    {
        if (_idlePos || weaponContext.AttackReleased)
        {
            transform.localPosition = weaponContext.WeaponPosActive.position;
            _idlePos = false;
        }
    }
    /// <summary>
    /// Set position to active position object
    /// </summary>
    /// <param name="weaponContext">weaponContext for Position</param>
    private void SetIdlePose(WeaponContext weaponContext)
    {
        if (!_idlePos || !weaponContext.AimMode)
        {
            transform.localPosition = weaponContext.WeaponPosIdle.position;
            _idlePos = true;
        }
    }
    /// <summary>
    /// Called once when Weapon is created
    /// </summary>
    /// <param name="weaponContext"></param>
    public void OnInitialize(WeaponContext weaponContext)
    {
        if (_ammoDataObject == null || _ammoDataObject.Count == 0)
        {
            return;
        }
        if (GetComponent<Spawner>() == null)
            this.AddComponent<Spawner>();
        _spawner = GetComponent<Spawner>();

        for (int i = 0; i < _ammoDataObject.Count; i++)
        {
            _ammoCounts[i] = _weaponData.maxAmmo;
        }
        _currentAmmoCount = _weaponData.maxAmmo;
        EquipAmmo(0);
        Equip(weaponContext);
    }
    #endregion
}