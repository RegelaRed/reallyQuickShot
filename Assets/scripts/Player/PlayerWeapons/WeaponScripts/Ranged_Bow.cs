using UnityEngine;

public class Ranged_Bow : MonoBehaviour, IWeapons
{
    [SerializeField] private WeaponData _data;
    public WeaponData Data => _data;
    public void Enter() { }
    public void Exit() { }
    public void Attack() { }
    public void UpdateWeapon() { }

    public void Reload() { }
    public void SwitchAmmo() { }
}
