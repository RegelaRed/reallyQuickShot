using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _playerInput;
    [SerializeField] private Transform _weaponPositionIdle;
    [SerializeField] private Transform _weaponPositionActive;
    [SerializeField] private List<GameObject> _weaponPrefabs;

    private List<IWeapons> _weaponList = new List<IWeapons>();
    private int _currentWeaponIndex;
    private IWeapons _currentWeapon;

    [SerializeField] private float idleTime = 10f;
    private float _idleTimer;

    public PlayerInputHandler Input { get { return _playerInput; } }

    private void Awake()
    {
        if (_playerInput == null) _playerInput = GetComponent<PlayerInputHandler>();

        foreach (var prefab in _weaponPrefabs)
        {
            GameObject weaponObj = Instantiate(prefab, _weaponPositionIdle);
            IWeapons weapon = weaponObj.GetComponent<IWeapons>();
            if (weapon != null)
            {
                _weaponList.Add(weapon);
                weaponObj.SetActive(false);
            }
            else
            {
                Destroy(weaponObj);
            }
        }
    }
    private void Update()
    {
        if (_currentWeapon != null)
            _currentWeapon.UpdateWeapon();
        else
            Debug.Log($"current weapon is null");
        SwitchWeapon();
    }

    void SwitchWeapon()
    {
        if (_currentWeapon is Ranged_Bow)
        {
            EquipWeapon(0);
        }

    }
    void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weaponList.Count) return;
        if (_currentWeapon != null)
            _currentWeapon.Exit();

        _currentWeaponIndex = index;
        _currentWeapon = _weaponList[index];
        _currentWeapon.Enter();
    }
}