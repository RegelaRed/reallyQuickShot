using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    private IHittable[] _hittables;

    private void Awake()
    {
        // Cache all hit handlers (no allocation later)
        _hittables = GetComponents<IHittable>();

        if (_hittables.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name} has HitReceiver but no IHittable components!", this);
        }
    }

    public void ReceiveHit(ProjectileHitData hitData)
    {
        // Notify all handlers
        foreach (var hittable in _hittables)
        {
            hittable.OnHit(hitData);
        }
    }
}
