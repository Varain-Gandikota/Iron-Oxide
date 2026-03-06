using UnityEngine;

public class GunSpriteChanger : MonoBehaviour
{
    [SerializeField] private Animator gunAnimator;
    void FixedUpdate()
    {
        Vector2 direction = transform.right;
        gunAnimator.SetFloat("X", direction.x);
        gunAnimator.SetFloat("Y", direction.y);
    }

}
