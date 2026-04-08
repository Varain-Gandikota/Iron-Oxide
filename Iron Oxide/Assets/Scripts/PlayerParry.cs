using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] Transform localParticleSystemPosition;
    [SerializeField] private Animator gunHolderAnimator;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private bool canParry = true;

    [SerializeField] private float parryCoolDown = 0.3f;
    [SerializeField] private float successParryCoolDown = 0.1f;
    [SerializeField] private ParticleSystem parryEffect;
    
    private bool isOnCoolDown = false;
    // ignores the cooldown variable if its true
    private bool cancelCoolDown = false;
    private bool doParry = false;
    private Collider2D currentlyParriedCollider = null;
    private readonly int AnimatorRTLParryHash = Animator.StringToHash("RTL Parry");
    private readonly int AnimatorLTRParryHash = Animator.StringToHash("LTR Parry");
    private InputAction meleeAction;

    private bool isRightAnimation = true;
    private int parryLayer = 0;

    private Coroutine parryCoolDownCoroutine;
    private Coroutine stopGunEffectCoroutine;
    private Vector3 originalLocalPosition;
    public Action OnParrySuccess = delegate { };
    public Action OnParryFail = delegate { };
    [SerializeField] private float parryEffectShakeStrength = 390f;
    private void Awake()
    {
        meleeAction = InputSystem.actions.FindAction("Melee");
        meleeAction.performed += AttemptParry;
        parryLayer = LayerMask.NameToLayer("Parry");
    }
    private void Start()
    {
        originalLocalPosition = transform.localPosition;
    }
    public void AttemptParry(InputAction.CallbackContext context)
    {
        doParry = canParry && (!isOnCoolDown || cancelCoolDown);
        //Debug.Log("Parry Attempted. Can Parry: " + canParry + ", Is On Cooldown: " + isOnCoolDown + ", Cancel Cooldown: " + cancelCoolDown + ", Do Parry: " + doParry);
        if (doParry) {
            isRightAnimation = !isRightAnimation;
            gunHolderAnimator.Play(isRightAnimation ? AnimatorRTLParryHash : AnimatorLTRParryHash, 0, 0f);

            if (parryCoolDownCoroutine != null)
            {
                StopCoroutine(parryCoolDownCoroutine);
            }
            parryCoolDownCoroutine = StartCoroutine(ParryCoolDown(parryCoolDown));
        }
        
    }
    private IEnumerator ParryCoolDown(float time)
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(time);
        isOnCoolDown = false;
        currentlyParriedCollider = null;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == parryLayer)
        {
            if (collision == currentlyParriedCollider)
            {
                return;
            }
            Debug.Log("Parry Attempt Detected With " + collision.gameObject.name);
            if (doParry) {
                currentlyParriedCollider = collision;
                Debug.Log("Parried Detected With " + collision.gameObject.name);
                if (parryCoolDownCoroutine != null)
                {
                    StopCoroutine(parryCoolDownCoroutine);
                }
                parryCoolDownCoroutine = StartCoroutine(ParryCoolDown(successParryCoolDown));
                parryEffect.transform.position = localParticleSystemPosition.transform.position;
                parryEffect.Play();
                OnParrySuccess.Invoke();
                if (stopGunEffectCoroutine != null)
                {
                    StopCoroutine(stopGunEffectCoroutine);
                }
                stopGunEffectCoroutine = StartCoroutine(StartGunParryEffects(0.3f));

            }

        }

    }
    private IEnumerator StartGunParryEffects(float time)
    {
        ShakeObject.Shake(parryEffectShakeStrength, time, gunHolderAnimator.transform, originalLocalPosition);
        //gunHolderAnimator.StopPlayback();
        //gunHolderAnimator.speed = 0;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        //gunHolderAnimator.speed = 1;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == parryLayer)
        {
            currentlyParriedCollider = null;
        }
    }
}

