using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileData _stats;
    private Rigidbody _rb;
    private float _aliveTime;
    private bool _hasHit;

    public ProjectileData Stats => _stats;
    public bool ShouldDestroy => _aliveTime >= _stats.lifeTime || _hasHit;

    /// <summary>
    /// sets all necessary data on creation
    /// </summary>
    /// <param name="stats"> ProjectileData for initializing behaviour</param>
    /// <param name="velocity"> speed of projectile </param>
    public void Initialize(ProjectileData stats, Vector3 velocity)
    {
        _stats = stats;
        _rb = GetComponent<Rigidbody>();

        // Apply stats to rigidbody
        _rb.useGravity = stats.useGravity;
        _rb.drag = stats.drag;
        _rb.velocity = velocity;

    }

    private void FixedUpdate()
    {
        //set the prefab facing direction
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit) return;

        // Check if we should hit this layer
        if ((_stats.hitLayers.value & (1 << collision.gameObject.layer)) == 0)
            return;

        _hasHit = true;

        // Build hit data
        ContactPoint contact = collision.contacts[0];
        ProjectileHitData hitData = new ProjectileHitData
        {
            hitPoint = contact.point,
            hitNormal = contact.normal,
            damage = _stats.damage,
            projectile = gameObject,
            velocity = _rb.velocity
        };

        // Notify target
        var receiver = collision.collider.GetComponent<HitReciver>();
        if (receiver != null)
        {
            receiver.ReceiveHit(hitData);
        }

        // Spawn hit effect
        if (_stats.hitEffectPrefab != null)
        {
            Instantiate(_stats.hitEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        }

        // Play hit sound
        if (_stats.hitAudio != null)
        {
            AudioSource.PlayClipAtPoint(_stats.hitAudio, contact.point);
        }

        // Stick or destroy
        if (_stats.stickToSurface)
        {
            StickToSurface(collision);
        }
        else if (_stats.destroyOnHit)
        {
            Destroy(gameObject, 5f);
        }
    }
    /// <summary>
    /// Projectile Stick to surface
    /// </summary>
    /// <param name="collision"></param>
    private void StickToSurface(Collision collision)
    {
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;

        // Parent to hit object (moves with it)
        transform.SetParent(collision.transform);

        // Destroy after delay
        Destroy(gameObject, 3f);
    }
}