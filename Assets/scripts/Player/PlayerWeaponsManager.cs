using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("References")]

    //Script References
    [SerializeField] private PlayerInputHandler _input;
    [SerializeField] private PlayerVariables _variables;

    //Transforms and prefabs

    [SerializeField] private Transform _weaponIdlePos;
    [SerializeField] private Transform _weaponActivePos;

    [SerializeField] List<GameObject> _weaponPrefabs;
    //weapon switching
    private AttackStatebase _currentAttackState;
    private AttackStateFactory _attackStateFactory;

    private List<IWeapons> _weapons = new List<IWeapons>();
    private IWeapons _currentWeapon;
    private int _currentWeaponIndex = 0;

    //Getters/Setters
    public PlayerInputHandler Input { get { return _input; } }
    public PlayerVariables Variables { get { return _variables; } }

    public Transform WeaponIdlePositon { get { return _weaponIdlePos; } }
    public Transform WeaponActivePositon { get { return _weaponActivePos; } }

    public IWeapons CurrentWeapons { get { return _currentWeapon; } }
    public AttackStatebase CurrentAttackState { get { return _currentAttackState; } set { _currentAttackState = value; } }

    private void Awake()
    {
        //Input 
        if (_input == null) _input = GetComponent<PlayerInputHandler>();

        //initialize current weapons
        foreach (var prefab in _weaponPrefabs)
        {
            IWeapons weapon = prefab.GetComponent<IWeapons>();
            if (weapon != null)
                _weapons.Add(weapon);
        }
        if (_weapons.Count > 0)
            EquiupWeapon(0);

        //AtatckMode SM
        _attackStateFactory = new AttackStateFactory(this);
        _currentAttackState = _attackStateFactory.Idle();
        _currentAttackState.Enter();
    }

    private void Update()
    {
        _currentAttackState.UpdateWeapon();
    }

    private void EquiupWeapon(int index)
    {
        if (index < 0 || index > _weapons.Count) return;

        _currentWeapon.Unequip();

        _currentWeaponIndex = index;
        _currentWeapon = _weapons[index];
        _currentWeapon.Equip();

        if (_currentWeapon.Type == WeaponTypeEnum.Melee)
            _currentAttackState.SwitchWeapon(_attackStateFactory.Melee());
        else if (_currentWeapon.Type == WeaponTypeEnum.Ranged)
            _currentAttackState.SwitchWeapon(_attackStateFactory.Ranged());
    }

    public void SwitchToNextWeapon()
    {
        int nextIndex = (_currentWeaponIndex + 1) % _weapons.Count;
        EquiupWeapon(nextIndex);
    }
    public void CreateProjectile(GameObject projectile, Vector3 direction, float speed)
    {

    }
}