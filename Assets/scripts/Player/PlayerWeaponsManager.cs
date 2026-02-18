using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    // ─────────────── Scene References ─────────────── 

    [Header("Scene References")]
    [SerializeField] private PlayerInputHandler _playerInput;
    [SerializeField] private Spawner _spawner;

    [SerializeField] private Transform _weaponPositionIdle;
    [SerializeField] private Transform _weaponPositionActive;
    [SerializeField] private Transform _orientation;

    // ─────────────── Weapon Data ─────────────── 

    [Header("Weapon List")]
    [SerializeField] private List<GameObject> _weaponPrefabs;

    private readonly List<WeaponsBase> _weaponList = new();
    private int _currentWeaponIndex;
    private WeaponsBase _currentWeapon;

    // ─────────────── Input ─────────────── 
    public WeaponContext _weaponContext = new WeaponContext();

    // ─────────────── Unity Lifecycle ─────────────── 

    private void Awake()
    {
        _playerInput ??= GetComponent<PlayerInputHandler>();
        _spawner ??= GetComponent<Spawner>();

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

        if (_playerInput.SwitchWeaponPressed)
            SwitchWeaponIndex();
    }

    // ─────────────── Initialization ─────────────── 
    /// <summary>
    /// Instanciate all Weapon Instances and Hides them
    /// </summary>
    private void InitializeWeapons()
    {
        foreach (var prefab in _weaponPrefabs)
        {
            GameObject weaponObj = Instantiate(prefab, _weaponPositionIdle);
            WeaponsBase weapon = weaponObj.GetComponent<WeaponsBase>();
            weaponObj.transform.position = Vector3.zero;

            if (weapon == null)
            {
                Destroy(weaponObj);
                continue;
            }
            Debug.Log($"Instanciated weapon name: {weaponObj.name}");
            _weaponList.Add(weapon);
            weaponObj.transform.localPosition = Vector3.zero;
            weapon.OnInitialize(this);
            weapon.UnEquip(_weaponContext);
        }
    }

    // ─────────────── Input ─────────────── 

    /// <summary>
    /// Snapshot of Input for Weapons to read
    /// </summary>
    private void CaptureInput()
    {
        bool _wasAttackHeld = _playerInput.AttackHeld;

        _weaponContext.DeltaTime = Time.deltaTime;

        _weaponContext.AttackDirection = _orientation.forward;

        _weaponContext.AttackPressed = _playerInput.AttackPressed && !_wasAttackHeld;
        _weaponContext.AttackHeld = _playerInput.AttackHeld;
        _weaponContext.AttackReleased = !_playerInput.AttackHeld && _wasAttackHeld;

        _weaponContext.ReloadPressed = _playerInput.ReloadPressed;

        _weaponContext.AmmoNext = _playerInput.AmmoNext;
        _weaponContext.AmmoPrevious = _playerInput.AmmoPrevious;

        _weaponContext.AimMode = _playerInput.IsAiming;
    }

    // ─────────────── Weapon Switching ─────────────── 
    /// <summary>
    /// Switch Weapon Index, Increments index on each call
    /// </summary>
    private void SwitchWeaponIndex()
    {
        int nextIndex = (_currentWeaponIndex + 1) % _weaponList.Count;
        EquipWeapon(nextIndex);
    }
    /// <summary>
    /// Equip weapon prefab by Index
    /// </summary>
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
}
