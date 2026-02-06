using UnityEngine;

public class WeaponIdle : MonoBehaviour, IWeapons
{
    [SerializeField] private WeaponData _weaponData;
    public WeaponData Data => _weaponData;
    public void Attack() { }
    public void Enter() { }
    public void Exit() { }
    public void Reload() { }
    public void UpdateWeapon() { }
}
