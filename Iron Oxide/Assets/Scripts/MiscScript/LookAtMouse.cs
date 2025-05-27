using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    
    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position);
        transform.right = direction;
    }
}
