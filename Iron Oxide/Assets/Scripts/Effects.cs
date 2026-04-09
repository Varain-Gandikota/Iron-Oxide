using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effects : MonoBehaviour
{
    [Header("Shake Effect")]
    public AnimationCurve curve;
    public static float shakeObjectConstant = 0.075f;
    
    private static Dictionary<Transform, (Vector3, Coroutine)> currentCoroutines = new Dictionary<Transform, (Vector3, Coroutine)>();
    private static Effects effecter;

    private static Coroutine hitStopCoroutine;
    private static float originalTimeScale;

    private static Coroutine flashWhiteCoroutine;

    [Header("Flash White Effect")]
    [SerializeField] private RawImage flashWhiteImage; 
    public void Awake()
    {
        effecter = this;
    }

    #region HelperFunctions
    private static void SafeStopCoroutine(Coroutine coroutine, Action resetFunction)
    {
        if (coroutine != null)
        {
            effecter.StopCoroutine(coroutine);
            resetFunction();
        }
    }
    #endregion

    #region HitStop
    public static void TriggerHitStop(float hitStopTime)
    {
        SafeStopCoroutine(hitStopCoroutine, () => Time.timeScale = originalTimeScale);
        hitStopCoroutine = effecter.StartCoroutine(effecter.HitStopCoroutine(hitStopTime));
    }
    private IEnumerator HitStopCoroutine(float hitStopTime)
    {
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = originalTimeScale;
    }
    #endregion

    #region ShakeEffect
    public static void Shake(float shakeStrength, float shakeTime, Transform objectToShake, Vector3 originalLocalPosition)
    {
        if (currentCoroutines.ContainsKey(objectToShake))
        {
            effecter.StopCoroutine(currentCoroutines[objectToShake].Item2);
            currentCoroutines[objectToShake] = (originalLocalPosition, effecter.StartCoroutine(effecter.ShakeRoutine(shakeStrength, shakeTime, objectToShake, originalLocalPosition)));
            objectToShake.localPosition = originalLocalPosition;
        } else {
            currentCoroutines.Add(objectToShake, (originalLocalPosition, effecter.StartCoroutine(effecter.ShakeRoutine(shakeStrength, shakeTime, objectToShake, originalLocalPosition))));
        }
    }
    private IEnumerator ShakeRoutine(float shakeStrength, float shakeTime, Transform objectToShake, Vector3 originalLocalPosition)
    {
        float time = 0f;
        while (time < shakeTime)
        {
            time += Time.deltaTime;
            float proportion = time / shakeTime;

            float xChange = Mathf.PerlinNoise(proportion, 0f) * shakeStrength * curve.Evaluate(proportion) * shakeObjectConstant * UnityEngine.Random.Range(-1, 2) * Time.deltaTime;
            float yChange = Mathf.PerlinNoise(0f, proportion) * shakeStrength * curve.Evaluate(proportion) * shakeObjectConstant * UnityEngine.Random.Range(-1, 2) * Time.deltaTime;
            objectToShake.transform.localPosition = originalLocalPosition + new Vector3(xChange, yChange, 0);

            yield return null;
        }
        objectToShake.transform.localPosition = originalLocalPosition;
        currentCoroutines.Remove(objectToShake);
    }
    #endregion

    #region FlashWhiteEffect
    public static void FlashWhite(float flashDuration = 0.15f, float fadeDuration = 0.2f)
    {
        SafeStopCoroutine(flashWhiteCoroutine, () => effecter.flashWhiteImage.color = new Color(1f, 1f, 1f, 0f));
        flashWhiteCoroutine = effecter.StartCoroutine(effecter.FlashWhiteRoutine(flashDuration, fadeDuration));
    }

    private IEnumerator FlashWhiteRoutine(float flashDuration, float fadeDuration)
    {
        flashWhiteImage.color = new Color(1f, 1f, 1f, 0.15f);

        yield return new WaitForSecondsRealtime(flashDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0.15f, 0f, elapsedTime / fadeDuration);
            flashWhiteImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        flashWhiteImage.color = new Color(1f, 1f, 1f, 0f);
    }

    #endregion
}
