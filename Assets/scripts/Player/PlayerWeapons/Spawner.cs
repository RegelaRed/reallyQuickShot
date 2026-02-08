using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject _currentObject;
    public void CreateProjectile(float speed, Transform startPosition, GameObject prefab)
    {
        _currentObject = Instantiate(prefab, startPosition.position, startPosition.rotation);
        Rigidbody rb = _currentObject.GetComponent<Rigidbody>();
        rb.AddForce(startPosition.forward.normalized * speed);
        Destroy(_currentObject, 8f);
    }
}