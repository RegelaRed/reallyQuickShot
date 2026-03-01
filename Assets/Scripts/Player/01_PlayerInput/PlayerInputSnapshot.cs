
using UnityEngine;

public struct PlayerInputSnapshot
{
    // ------------ Movement ------------ 
    public Vector2 Move;
    public bool MovePressed;
    public Vector2 Look;

    // ------------ Sprint ------------  
    public bool SprintPressed;
    public bool SprintToggle;
    // ------------ Jump ------------ 
    public bool JumpPressed;
    public bool JumpHeld;
    // ------------ Dash ------------ 
    public bool DashPressed;
    public bool DashHeld;
    public Vector3 DashDirection;
    // ------------ Attack ------------ 
    public bool AttackHeld;
    public bool AttackPressed;
    public bool AttackReleased;
    public bool ReloadPressed;
    // ------------ Weapon switching ------------ 
    public bool AmmoPrevious;
    public bool AmmoNext;
    public bool SwitchWeapon;
    // -------- Aim Mode --------
    public bool AimMode;
}
