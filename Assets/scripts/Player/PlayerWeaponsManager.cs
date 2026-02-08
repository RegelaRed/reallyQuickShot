using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playercontroller;
    [SerializeField] private PlayerInputHandler _playerInput;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Transform _weaponPositionIdle;
    [SerializeField] private Transform _weaponPositionActive;
    [SerializeField] private List<GameObject> _weaponPrefabs;

    private List<WeaponsBase> _weaponList;
    private int _currentWeaponIndex;
    private WeaponsBase _currentWeapon;

    public WeaponInput InputSnapshot { get; private set; }


    public Transform WeaponPositionIdle { get { return _weaponPositionIdle; } }
    public Transform WeaponPositionActive { get { return _weaponPositionActive; } }
    public PlayerController Controller { get { return _playercontroller; } }
    public PlayerInputHandler Input { get { return _playerInput; } }
    public Spawner Spawner { get { return _spawner; } }

    private void Awake()
    {
        _weaponList = new List<WeaponsBase>();

        if (_playercontroller == null) _playercontroller = GetComponent<PlayerController>();
        if (_playerInput == null) _playerInput = GetComponent<PlayerInputHandler>();
        if (_spawner == null) _spawner = GetComponent<Spawner>();

        foreach (var prefab in _weaponPrefabs)
        {
            // Debug.Log($"Avalable prefabs {prefab.name}");
            GameObject weaponObj = Instantiate(prefab, _weaponPositionIdle);
            WeaponsBase weapon = weaponObj.GetComponent<WeaponsBase>();
            if (weapon != null)
            {
                _weaponList.Add(weapon);
                weapon.OnInitialize(this);
                weapon.UnEquip();
            }
            else
            {
                Destroy(weaponObj);
            }
        }
        EquipWeapon(0);
    }
    void Update()
    {
        InputSnapshot = new WeaponInput
        {
            AttackHeld = Input.AttackHeld,
            AttackReleased = !Input.AttackHeld,
            ReloadPressed = Input.ReloadPressed,
            AmmoNext = Input.AmmoNext
        };

        if (_currentWeapon != null) _currentWeapon.UpdateWeapon(InputSnapshot);
        if (_playerInput.SwitchWeaponPressed) SwitchWeapon();
    }
    private void SwitchWeapon()
    {
        int index = (_currentWeaponIndex + 1) % _weaponList.Count;
        EquipWeapon(index);
    }
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weaponList.Count) { Debug.Log("IndexOutOfRangeException"); return; }

        if (_currentWeapon != null) _currentWeapon.UnEquip();

        _currentWeaponIndex = index;
        _currentWeapon = _weaponList[index];
        _currentWeapon.Equip();
    }
}