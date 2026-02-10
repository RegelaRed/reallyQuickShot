using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileData _stats;
    private Rigidbody _rb;
    private float _aliveTime;
    private bool _hasHit;

    public ProjectileData Stats => _stats;
    public bool ShouldDestroy => _aliveTime >= _stats.lifeTime || _hasHit;

    public void Initialize(ProjectileData stats, Vector3 velocity)
    {
        _stats = stats;
        _rb = GetComponent<Rigidbody>();

        // Apply stats to rigidbody
        _rb.useGravity = stats.useGravity;
        _rb.drag = stats.drag;
        _rb.velocity = velocity;

        if (stats.useGravity && stats.gravityMultiplier != 1f)
        {
            _rb.useGravity = false; // We'll apply custom gravity
        }
    }

    private void FixedUpdate()
    {
        if (_hasHit) return;

        _aliveTime += Time.fixedDeltaTime;

        // Custom gravity multiplier
        if (_stats.useGravity && _stats.gravityMultiplier != 1f)
        {
            _rb.AddForce(Physics.gravity * _stats.gravityMultiplier, ForceMode.Acceleration);
        }

        // Point projectile in direction of travel
        if (_rb.velocity.magnitude > 0.1f)
        {
            transform.forward = _rb.velocity.normalized;
        }
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
            Destroy(gameObject);
        }
    }

    private void StickToSurface(Collision collision)
    {
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;

        // Parent to hit object (moves with it)
        transform.SetParent(collision.transform);

        // Destroy after delay
        Destroy(gameObject, 3f);
    }
}