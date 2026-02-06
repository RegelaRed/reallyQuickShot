using System;

public interface IWeapons
{
    public WeaponData Data { get; }
    public WeaponType weaponType { get => Data.type; }
    void Enter();
    void Exit();
    void Attack();
    void UpdateWeapon();
}
