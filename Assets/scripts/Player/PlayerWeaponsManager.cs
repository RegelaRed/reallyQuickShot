using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    // ─────────────── Scene References ─────────────── 

    [Header("Scene References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInputHandler _playerInput;
    [SerializeField] private Spawner _spawner;

    [SerializeField] private Transform _weaponPositionIdle;
    [SerializeField] private Transform _weaponPositionActive;

    // ─────────────── Weapon Data ─────────────── 

    [Header("Weapon List")]
    [SerializeField] private List<GameObject> _weaponPrefabs;

    private readonly List<WeaponsBase> _weaponList = new();
    private int _currentWeaponIndex;
    private WeaponsBase _currentWeapon;

    // ─────────────── Input ─────────────── 

    public WeaponInput InputSnapshot { get; private set; }

    // ─────────────── Public Access ─────────────── 

    public Transform WeaponPositionIdle => _weaponPositionIdle;
    public Transform WeaponPositionActive => _weaponPositionActive;

    public PlayerController Controller => _playerController;
    public PlayerInputHandler Input => _playerInput;
    public Spawner Spawner => _spawner;

    // ─────────────── Unity Lifecycle ─────────────── 

    private void Awake()
    {
        _playerController ??= GetComponent<PlayerController>();
        _playerInput ??= GetComponent<PlayerInputHandler>();
        _spawner ??= GetComponent<Spawner>();

        InitializeWeapons();
        EquipWeapon(0);
    }
    private void Update()
    {
        CaptureInput();

        _currentWeapon?.UpdateWeapon(InputSnapshot);

        if (_playerInput.SwitchWeaponPressed)
            SwitchWeapon();
    }

    // ─────────────── Initialization ─────────────── 

    private void InitializeWeapons()
    {
        foreach (var prefab in _weaponPrefabs)
        {
            GameObject weaponObj = Instantiate(prefab, _weaponPositionIdle);
            WeaponsBase weapon = weaponObj.GetComponent<WeaponsBase>();

            if (weapon == null)
            {
                Destroy(weaponObj);
                continue;
            }

            _weaponList.Add(weapon);
            weaponObj.transform.localPosition = Vector3.zero;
            weapon.OnInitialize(this);
            weapon.UnEquip();
        }
    }

    // ─────────────── Input ─────────────── 

    private void CaptureInput()
    {
        bool _wasAttackHeld = Input.AttackHeld;
        InputSnapshot = new WeaponInput
        {
            AttackPressed = Input.AttackPressed && !_wasAttackHeld,
            AttackHeld = Input.AttackHeld,
            AttackReleased = !Input.AttackHeld && _wasAttackHeld,

            ReloadPressed = Input.ReloadPressed,
            AmmoNext = Input.AmmoNext
        };
        _wasAttackHeld = Input.AttackHeld;
    }

    // ─────────────── Weapon Switching ─────────────── 

    private void SwitchWeapon()
    {
        int nextIndex = (_currentWeaponIndex + 1) % _weaponList.Count;
        EquipWeapon(nextIndex);
    }
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weaponList.Count)
        {
            Debug.LogError("Weapon index out of range");
            return;
        }

        _currentWeapon?.UnEquip();

        _currentWeaponIndex = index;
        _currentWeapon = _weaponList[index];
        _currentWeapon.Equip();
    }
}
