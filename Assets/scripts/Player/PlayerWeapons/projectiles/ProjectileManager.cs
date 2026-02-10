using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager _instance;
    public static ProjectileManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("ProjectileManager");
                _instance = go.AddComponent<ProjectileManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private List<Projectile> _activeProjectiles = new List<Projectile>();

    public Projectile Spawn(ProjectileData stats, Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        if (stats.prefab == null)
        {
            Debug.Log("Projectile Prefab empty"); return null;
        }
        GameObject obj = Instantiate(stats.prefab, position, rotation);
        Projectile projectile = obj.GetComponent<Projectile>();

        if (projectile == null)
        {
            obj.AddComponent<Projectile>();
        }
        projectile.Initialize(stats, velocity);
        _activeProjectiles.Add(projectile);
        return projectile;
    }

    private void Update()
    {
        for (int i = _activeProjectiles.Count - 1; i >= 0; i--)
        {
            if (_activeProjectiles[i] == null || _activeProjectiles[i].ShouldDestroy)
            {
                if (_activeProjectiles[i] != null)
                {
                    Destroy(_activeProjectiles[i].gameObject);
                }
                _activeProjectiles.RemoveAt(i);
            }
        }
    }
    public void DestroyAll()
    {
        foreach (var proj in _activeProjectiles)
        {
            if (proj != null)
                Destroy(proj.gameObject);
        }
        _activeProjectiles.Clear();
    }

    public int GetActiveCount() => _activeProjectiles.Count;

    public void PauseAll()
    {
        foreach (var proj in _activeProjectiles)
        {
            if (proj != null)
                proj.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void ResumeAll()
    {
        foreach (var proj in _activeProjectiles)
        {
            if (proj != null)
                proj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
