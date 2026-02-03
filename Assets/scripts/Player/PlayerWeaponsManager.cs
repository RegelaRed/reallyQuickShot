using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    //Script References
    [SerializeField] private PlayerVariables _variables;
    [SerializeField] private Transform _weaponPosition;
    private PlayerInputHandler _input;
    private AttackStatebase _currentAttackMode;
    private AttackStateFactory _attackModeFactory;

    private List<IWeapons> _weapons;
    private IWeapons _currentWeapon;

    //Getters/Setters
    public Transform WeaponPositon { get { return _weaponPosition; } }

    public PlayerInputHandler Input { get { return _input; } }
    public PlayerVariables Variables { get { return _variables; } }
    public AttackStatebase CurrentWeapon { get { return _currentAttackMode; } set { _currentAttackMode = value; } }

    private void Awake()
    {
        if (_input == null) _input = GetComponent<PlayerInputHandler>();
        //AtatckMode SM
        _attackModeFactory = new AttackStateFactory(this);
        _currentAttackMode = _attackModeFactory.Idle();
        _currentAttackMode.Enter();

        _currentWeapon = _weapons[0];
    }
}