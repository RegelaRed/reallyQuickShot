using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    #region References
    // ─────────────── Scene References ─────────────── 

    [Header("Scene References")]

    [SerializeField] private Transform _weaponPositionIdle;
    [SerializeField] private Transform _weaponPositionActive;
    [SerializeField] private Transform _orientation;
    private PlayerInputHandler _playerInput;
    private Spawner _spawner;

    // ─────────────── Weapon Data ─────────────── 

    [Header("Weapon List")]
    [SerializeField] private List<GameObject> _weaponPrefabs;

    private readonly List<WeaponsBase> _weaponList = new();
    private int _currentWeaponIndex;
    private WeaponsBase _currentWeapon;

    // ─────────────── Input ─────────────── 
    public WeaponContext _weaponContext = new WeaponContext();
    private bool _wasAttackHeld = false;

    #endregion
    #region Updates 

    private void Awake()
    {
        _playerInput ??= GetComponent<PlayerInputHandler>() ?? this.AddComponent<PlayerInputHandler>();
        _spawner ??= GetComponent<Spawner>() ?? this.AddComponent<Spawner>();

        _weaponContext.Spawner = _spawner;
        _weaponContext.Orientation = _orientation;
        _weaponContext.WeaponPosIdle = _weaponPositionIdle;
        _weaponContext.WeaponPosActive = _weaponPositionActive;

        InitializeWeapons();
        EquipWeapon(0);
    }
    private void Update()
    {
        CaptureInput();

        _currentWeapon?.UpdateWeapon(_weaponContext);

        if (_playerInput.WeaponNext)
            SwitchWeaponIndex();
        else if (_playerInput.WeaponPrevious)
            SwitchWeaponIndex(-1);
    }
    #endregion
    #region Initialization 

    /// <summary>Instanciate all Weapon Instances and Hides them</summary>
    private void InitializeWeapons()
    {
        foreach (var prefab in _weaponPrefabs)
        {
            GameObject weaponObj = Instantiate(prefab, _weaponPositionIdle, false);
            WeaponsBase weapon = weaponObj.GetComponent<WeaponsBase>();

            if (weapon == null)
            {
                Destroy(weaponObj);
                continue;
            }
            _weaponList.Add(weapon);
            weapon.OnInitialize(_weaponContext);
            weapon.UnEquip(_weaponContext);
        }
    }

    #endregion
    #region Input 

    /// <summary>Snapshot of Input for Weapons to read</summary>
    private void CaptureInput()
    {
        _weaponContext.DeltaTime = Time.deltaTime;

        _weaponContext.AttackDirection = _orientation.forward;

        _weaponContext.AttackPressed = _playerInput.AttackPressed && !_wasAttackHeld;
        _weaponContext.AttackHeld = _playerInput.AttackHeld;
        _weaponContext.AttackReleased = !_playerInput.AttackHeld && _wasAttackHeld;

        _weaponContext.ReloadPressed = _playerInput.ReloadPressed;

        _weaponContext.WeaponPrevious = _playerInput.WeaponPrevious;
        _weaponContext.WeaponNext = _playerInput.WeaponNext;

        _weaponContext.AmmoSwitch = _playerInput.SwitchAmmoPressed;

        _weaponContext.AimMode = _playerInput.IsAiming;

        _wasAttackHeld = _playerInput.AttackHeld;
    }

    #endregion
    #region Weapon Switching

    /// <summary>Switch Weapon Index, Increments index on each call</summary>
    private void SwitchWeaponIndex(int index = 1)
    {
        int nextIndex = (_currentWeaponIndex + index + _weaponList.Count) % _weaponList.Count;
        EquipWeapon(nextIndex);
    }

    /// <summary>Equip weapon prefab by Index</summary>
    /// <param name="index"> int Index of weapon to be Selected </param>
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weaponList.Count)
        {
            Debug.LogError("Weapon index out of range");
            return;
        }

        _currentWeapon?.UnEquip(_weaponContext);

        _currentWeaponIndex = index;
        _currentWeapon = _weaponList[index];
        _currentWeapon.Equip(_weaponContext);
    }
    #endregion
}
