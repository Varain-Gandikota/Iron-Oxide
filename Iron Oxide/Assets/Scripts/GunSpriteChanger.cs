using UnityEngine;

public class GunSpriteChanger : MonoBehaviour
{
    private Animator gunAnimator;
    private void Start()
    {
        gunAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position);
        gunAnimator.SetFloat("X", direction.x);
        gunAnimator.SetFloat("Y", direction.y);
    }
}
