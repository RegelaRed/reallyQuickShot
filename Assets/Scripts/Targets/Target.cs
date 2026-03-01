using UnityEngine;

public class Target : MonoBehaviour, IHittable
{
    public void OnHit(ProjectileHitData hitData)
    {
        Debug.Log($"Target was hit for {hitData.damage} Damage");
    }
}
