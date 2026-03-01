using UnityEngine;

[CreateAssetMenu(menuName = "ProjectileData/Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Damage")]
    public LayerMask hitLayers;
    public int damage;

    [Header("Lifetime")]
    public float lifeTime;

    [Header("Physics")]
    public float speed;
    public bool useGravity;
    public float gravityMultiplier;
    public float drag;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public AudioClip hitAudio;

    [Header("Behaviour")]
    public bool destroyOnHit;
    public bool stickToSurface;
}
