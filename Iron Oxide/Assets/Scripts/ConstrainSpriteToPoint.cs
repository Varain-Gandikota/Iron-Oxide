using UnityEngine;

public class ConstrainSpriteToPoint : MonoBehaviour
{
    [SerializeField] private Transform pointToLookAt;
    [SerializeField] private float accuracy = 0.15f;
    [SerializeField] private bool rotateTowards = false;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(ChangeSpriteWidthAndHeight), 0, accuracy);
    }

    private void ChangeSpriteWidthAndHeight()
    {
        Vector2 size = pointToLookAt.localPosition - transform.localPosition;
        if (Mathf.Abs(size.x) < 0.01f)
            size.x = .125f;
        if (Mathf.Abs(size.y) < 0.01f)
            size.y = .125f;
        if (rotateTowards)
        {
            transform.up = size.normalized;
        }
        spriteRenderer.size = size;
    }
}
