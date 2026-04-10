using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    void FixedUpdate()
    {
        Vector2 direction = (target.transform.position - transform.position);
        transform.right = direction;
    }
}
