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
            GameObject weaponObj = Instantiate(prefab, _weaponIdlePos);
            IWeapons weapon = weaponObj.GetComponent<IWeapons>();
            if (weapon != null)
            {
                _weapons.Add(weapon);
                weaponObj.SetActive(false);
            }
            else
            {
                Debug.Log($"Prefab {prefab.name} doesn't have IWeapons");
                Destroy(weaponObj);
            }
        }
        if (_weapons.Count > 0)
            EquipWeapon(0);

        //AtatckMode SM
        _attackStateFactory = new AttackStateFactory(this);
        _currentAttackState = _attackStateFactory.Idle();
        _currentAttackState.Enter();
    }

    private void Update()
    {
        _currentAttackState.UpdateWeapon();

        if (Input.WeaponNext)
            SwitchToNextWeapon(1);
        if (Input.WeaponPrevious)
            SwitchToNextWeapon(-1);
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Count) return;

        if (_currentWeapon != null) _currentWeapon.Unequip();

        _currentWeaponIndex = index;
        _currentWeapon = _weapons[index];
        _currentWeapon.Equip();


        switch (_currentWeapon.Type)
        {
            case WeaponTypeEnum.Idle:
                _currentAttackState.SwitchWeapon(_attackStateFactory.Idle());
                break;
            case WeaponTypeEnum.Melee:
                _currentAttackState.SwitchWeapon(_attackStateFactory.Melee());
                break;
            case WeaponTypeEnum.MeeleCharged:
                _currentAttackState.SwitchWeapon(_attackStateFactory.MeleeCharged());
                break;
            case WeaponTypeEnum.Ranged:
                _currentAttackState.SwitchWeapon(_attackStateFactory.Ranged());
                break;
            case WeaponTypeEnum.RangedCharged:
                _currentAttackState.SwitchWeapon(_attackStateFactory.RangedCharged());
                break;
            default:
                Debug.Log($"Current attack state is null");
                break;
        }
    }

    public void SwitchToNextWeapon(int ind)
    {
        int nextIndex = (_currentWeaponIndex + ind) % _weapons.Count;
        EquipWeapon(nextIndex);
    }
    public void CreateProjectile(GameObject projectile, Vector3 direction, float speed)
    {

    }
}