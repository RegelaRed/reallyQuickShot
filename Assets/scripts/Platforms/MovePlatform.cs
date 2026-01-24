using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private Transform platformBody;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private bool loop = false;
    [SerializeField] private float idleTime = 1f;
    private float idleTimer = 0;
    private Transform target;
    private bool isActive = false;

    public void Activate()
    {
        if (isActive) return;
        isActive = true;
        target = endPoint;
    }
    private void Move()
    {
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            return;
        }

        platformBody.position = Vector3.MoveTowards(platformBody.position, target.position, speed * Time.deltaTime);

        if ((platformBody.position - target.position).sqrMagnitude < 0.01f)
        {
            if (!loop)
            {
                isActive = false;
                return;
            }
            idleTimer = idleTime;
            target = target == startPoint ? endPoint : startPoint;
        }
    }
    private void Update()
    {
        if (!isActive)
            return;
        Move();
    }
}
