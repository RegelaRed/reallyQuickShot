using UnityEngine;

public class Melee_Knife : MonoBehaviour, IWeapons
{
    [SerializeField] private WeaponData _data;
    public WeaponData Data => _data;

    public void Enter() { }
    public void Exit() { }
    public void Attack() { }
    public void Reload() { }
    public void UpdateWeapon() { }
}
