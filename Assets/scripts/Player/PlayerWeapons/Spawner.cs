using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void CreateProjectile(ProjectileData projectileData, float chargePercentage, Transform startPosition)
    {
        Debug.Log($"Spawner Called {projectileData.prefab.name}");

        // Calculate velocity based on charge
        float minSpeed = projectileData.speed / 3;
        float maxSpeed = projectileData.speed;

        float speed = Mathf.Lerp(minSpeed, maxSpeed, chargePercentage);
        Vector3 velocity = startPosition.forward * speed;

        // Spawn through manager
        ProjectileManager.Instance.Spawn(
            projectileData,
            startPosition.position,
            startPosition.rotation,
            velocity
        );
    }
}