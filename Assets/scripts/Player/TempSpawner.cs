using UnityEngine;

public class TempSpawner : MonoBehaviour
{

    [SerializeField] private Spawner _spawner;
    [SerializeField] private ProjectileData _projectileData;
    [SerializeField] private Transform _startPos;
    [SerializeField] private float _fireRate;
    private float _fireRateTimer;
    private void Awake()
    {
        if (_spawner == null)
            Debug.Log($"Spawner not assigned");
    }

    private void Update()
    {
        if (_fireRateTimer > 0)
            _fireRateTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.J) && _fireRateTimer <= 0f)
        {
            _fireRateTimer = _fireRate;
            _spawner.CreateProjectile(_projectileData, 0.5f, _startPos);
        }
    }
}
