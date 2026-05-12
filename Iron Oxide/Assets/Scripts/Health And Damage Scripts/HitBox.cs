using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float knockbackForce = 5f; 
    [SerializeField] private int amountOfHits = 1;
    [SerializeField] private bool parryable = true;

    [SerializeField] private string tagToHit; 
    private bool canHit = true;
    private bool parried = false;
    private int totalHitsLeft = 1;
    private Collider2D hitboxCollider2D;

    private void Start()
    {
        canHit = amountOfHits > 0;
        hitboxCollider2D = GetComponent<Collider2D>();
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetState();
    }
    private void ResetState()
    {
        Debug.Log("HitBox disabled. Resetting parry and hit states.");
        parried = false;
        canHit = totalHitsLeft > 0;
        totalHitsLeft = amountOfHits;
    }
    public void GetParried()
    {
        if (parryable && !parried)
        {
            parried = true;
            canHit = totalHitsLeft > 0;
            totalHitsLeft--; 
            Debug.Log("HitBox has been parried! Remaining hits: " + totalHitsLeft);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canHit = !(parried && parryable) || totalHitsLeft > 0;
        if (!canHit) {
            Debug.Log("HitBox cannot hit anymore. No hits left.");
            return;
        }

        if (collision.gameObject.CompareTag(tagToHit))
        {
            //Debug.Log("HitBox hit a valid target: " + collision.gameObject.name);
            DamageManager.ApplyDamage(collision.gameObject, damageAmount);
            totalHitsLeft--;
            canHit = totalHitsLeft > 0;
            parried = false;
        }
    }
}
