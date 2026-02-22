using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void CreateProjectile(ProjectileData projectileData, float normalizedCharge, Transform startPosition)
    {
        // Debug.Log($"Spawner Called {projectileData.prefab.name}");

        // Calculate velocity based on charge
        float minSpeed = projectileData.speed / 3;
        float maxSpeed = projectileData.speed;
        float speed = Mathf.Lerp(minSpeed, maxSpeed, normalizedCharge);
        Vector3 velocity = startPosition.forward * speed;

        if (projectileData.prefab == null)
            return;

        GameObject obj = Instantiate(projectileData.prefab, startPosition.position, startPosition.rotation);
        Projectile projectile = obj.GetComponent<Projectile>();

        projectile.Initialize(projectileData, velocity);
    }
}