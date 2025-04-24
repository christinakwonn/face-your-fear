using UnityEngine;

public class GuardianBehavior : MonoBehaviour
{
    public Transform playerHead;
    public Camera playerCamera;
    public float moveSpeed = 1.5f;
    public float stopDistance = 1.0f;

    void Update()
    {
        bool inView = IsInView();
        bool hasLOS = HasLineOfSight();

        if (!inView || !hasLOS)
        {
            float dist = Vector3.Distance(playerHead.position, transform.position);
            if (dist > stopDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    playerHead.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }
    }

    bool IsInView()
    {
        Vector3 dirToEnemy = (transform.position - playerHead.position).normalized;
        float dot = Vector3.Dot(playerHead.forward, dirToEnemy);

        if (dot < 0.5f) return false;

        Vector3 viewPos = playerCamera.WorldToViewportPoint(transform.position);
        return viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
    }

    bool HasLineOfSight()
    {
        Vector3 dir = transform.position - playerCamera.transform.position;
        Ray ray = new Ray(playerCamera.transform.position, dir.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }

        return false;
    }
}
