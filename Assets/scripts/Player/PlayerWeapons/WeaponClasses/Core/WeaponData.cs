using UnityEngine;
[CreateAssetMenu(menuName = "WeaponData/Data")]
public class WeaponData : ScriptableObject
{
    public WeaponType type;

    public float fireRate;
    public float chargeRate;

    public float reloadTime;

    public float maxCharge;
    public int maxAmmo;
}