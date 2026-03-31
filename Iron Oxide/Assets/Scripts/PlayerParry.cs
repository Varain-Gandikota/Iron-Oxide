using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private Animator gunHolderAnimator;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float timeUntilReturnToIdle = 0.3f;
    [SerializeField] private bool canParry = true;

    [SerializeField] private float parryCoolDown = 0.3f;
    private bool isOnCoolDown = false;
    // ignores the cooldown variable if its true
    private bool cancelCoolDown = false;
    private bool doParry = false;
    private readonly int AnimatorRTLParryHash = Animator.StringToHash("RTL Parry");
    private readonly int AnimatorLTRParryHash = Animator.StringToHash("LTR Parry");

    private bool isRightAnimation = true;
    private int parryLayer = 0;

    private void Awake()
    {
        parryLayer = LayerMask.NameToLayer("Parry");
    }
    public void AttemptParry()
    {
        doParry = canParry && (!isOnCoolDown || cancelCoolDown);
        if (doParry) {
            isRightAnimation = !isRightAnimation;
            if (isRightAnimation) {
                gunHolderAnimator.Play(AnimatorRTLParryHash, 0, 0f);
            } 
            else {
                gunHolderAnimator.Play(AnimatorLTRParryHash, 0, 0f);
            }
            StartCoroutine(ParryCoolDown(parryCoolDown));
        }
        
    }
    private IEnumerator ParryCoolDown(float time)
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(time);
        isOnCoolDown = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == parryLayer)
        {
            Debug.Log("Parried Detected With " + collision.gameObject.name);
        }

    }
}
