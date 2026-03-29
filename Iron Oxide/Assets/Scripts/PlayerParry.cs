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

    private readonly int AnimatorParryHash = Animator.StringToHash("Parry");
    private readonly int AnimatorRTLParryHash = Animator.StringToHash("RTL Parry");
    private readonly int AnimatorLTRParryHash = Animator.StringToHash("LTR Parry");
    //private readonly int AnimatorReturnHash = Animator.StringToHash("Return");

    private bool isRightAnimation = true;
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


}
