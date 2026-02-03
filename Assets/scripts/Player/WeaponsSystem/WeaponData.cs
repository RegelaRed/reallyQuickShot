using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponStats")]
public class WeaponData : ScriptableObject
{
    public string weaponID;
    public float firerate;
    public int ammoCapacity;
}
