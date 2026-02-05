using UnityEngine;
[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponStats")]
public class WeaponData : ScriptableObject
{
    public WeaponTypeEnum type;
    public string weaponID;
    public float firerate;
    public int ammoCapacity;
    public float chargeRate;
    public float maxCharge;
}
