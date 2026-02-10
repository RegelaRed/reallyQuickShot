
using UnityEngine;

public struct PlayerInputSnapshot
{
    // ------------ Movement ------------ 
    public Vector2 Move;
    public Vector2 Look;
    // ------------ Jump ------------ 
    public bool JumpPressed;
    public bool JumpHeld;
    // ------------ Dash ------------ 
    public bool DashPressed;
    public bool DashHeld;
    // ------------ Attack ------------ 
    public bool AttackHeld;
    public bool AttackPressed;
    public bool AttackReleased;
    public bool ReloadPressed;
    // ------------ Weapon switching ------------ 
    public bool AmmoPrevious;
    public bool AmmoNext;
    public bool SwitchWeapon;
}
