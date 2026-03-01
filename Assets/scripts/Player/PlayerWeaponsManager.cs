using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    #region References
    // ─────────────── Scene References ─────────────── 

    [Header("Scene References")]

    [SerializeField] private Transform _weaponPositionIdle;
    [SerializeField] private Transform _weaponPositionActive;
    [SerializeField] private Transform _orientation;
    [SerializeField] private PlayerInputHandler _playerInput;
    [SerializeField] private Spawner _spawner;

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
        _weaponContext = CaptureInput();
        _wasAttackHeld = _playerInput.AttackHeld;

        _currentWeapon?.TickWeapon(_weaponContext);

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
        _weaponContext = CaptureInput();
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
    #region Input Snapshot

    /// <summary>Snapshot of Input for Weapons to read</summary>
    private WeaponContext CaptureInput()
    {
        return new WeaponContext
        {

            DeltaTime = Time.deltaTime,

            AttackDirection = _orientation.forward,
            AttackPressed = _playerInput.AttackPressed && !_wasAttackHeld,
            AttackHeld = _playerInput.AttackHeld,
            AttackReleased = !_playerInput.AttackHeld && _wasAttackHeld,

            ReloadPressed = _playerInput.ReloadPressed,

            WeaponPrevious = _playerInput.WeaponPrevious,
            WeaponNext = _playerInput.WeaponNext,

            AmmoSwitch = _playerInput.SwitchAmmoPressed,

            AimMode = _playerInput.IsAiming,
        };
    }

    #endregion
    #region Weapon Switching

    /// <summary>Switch Weapon Index, Increments index on each call</summary>
    private void SwitchWeaponIndex(int index = 1)
    {
        int nextIndex = (_currentWeaponIndex + index + _weaponList.Count) % _weaponList.Count;
        if (nextIndex == 0) nextIndex = 1;
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