using UnityEngine;

public class PlayerTorsoController : MonoBehaviour
{
    [SerializeField] private Animator torsoAnimator;

    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position) * 0.075f;
        Debug.Log(direction);
        torsoAnimator.SetFloat("X", direction.x);
        torsoAnimator.SetFloat("Y", direction.y);
    }
}
