using System.Collections;
using UnityEngine;
public class HitStopEffect : MonoBehaviour
{
    private static HitStopEffect instance;
    private static Coroutine hitStopCoroutine;
    private static float originalTimeScale;
    public void Awake()
    {
        instance = this;
    }
    public static void TriggerHitStop(float hitStopTime)
    {
        if (hitStopCoroutine != null)
        {
            instance.StopCoroutine(hitStopCoroutine);
            Time.timeScale = originalTimeScale;
        }
        hitStopCoroutine = instance.StartCoroutine(instance.HitStopCoroutine(0.15f));
    }
    private IEnumerator HitStopCoroutine(float hitStopTime)
    {
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = originalTimeScale;
    }
}
