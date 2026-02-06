using UnityEngine;
[CreateAssetMenu(menuName = "WeaponData/Data")]
public class WeaponData : ScriptableObject
{
    public string ID;
    public WeaponType type;

    public float fireRate;
    public float cahargeRate;

    public float maxCharge;
    public int maxAmmo;
}
