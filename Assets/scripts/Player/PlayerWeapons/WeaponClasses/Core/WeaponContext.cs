using UnityEngine;

public class WeaponContext
{
    // -------- Scene References --------
    public Transform Orientation;
    public Transform WeaponPosActive;
    public Transform WeaponPosIdle;
    public Spawner Spawner;
    // -------- Time --------
    public float DeltaTime;
    // -------- Attack --------
    public Vector3 AttackDirection;
    public bool AttackPressed;
    public bool AttackHeld;
    public bool AttackReleased;
    public bool ReloadPressed;
    // -------- Ammo Switching --------
    public bool AmmoNext;
    public bool AmmoPrevious;
    // -------- Aim Toggle --------
    public bool AimMode;
}