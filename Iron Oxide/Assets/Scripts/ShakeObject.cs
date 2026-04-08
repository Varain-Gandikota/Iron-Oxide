using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public AnimationCurve curve;
    public static float shakeObjectConstant = 0.075f;

    private static Dictionary<Transform, (Vector3, Coroutine)> currentCoroutines = new Dictionary<Transform, (Vector3, Coroutine)>();
    private static ShakeObject shaker;

    public void Awake()
    {
        shaker = this;
    }
    public static void Shake(float shakeStrength, float shakeTime, Transform objectToShake, Vector3 originalLocalPosition)
    {
        if (currentCoroutines.ContainsKey(objectToShake))
        {
            shaker.StopCoroutine(currentCoroutines[objectToShake].Item2);
            currentCoroutines[objectToShake] = (originalLocalPosition, shaker.StartCoroutine(shaker.ShakeRoutine(shakeStrength, shakeTime, objectToShake, originalLocalPosition)));
            objectToShake.localPosition = originalLocalPosition;
        } else {
            currentCoroutines.Add(objectToShake, (originalLocalPosition, shaker.StartCoroutine(shaker.ShakeRoutine(shakeStrength, shakeTime, objectToShake, originalLocalPosition))));
        }
            
        //currentCoroutines[objectToShake] = 
    }
    private IEnumerator ShakeRoutine(float shakeStrength, float shakeTime, Transform objectToShake, Vector3 originalLocalPosition)
    {
        float time = 0f;
        while (time < shakeTime)
        {
            time += Time.deltaTime;
            float proportion = time / shakeTime;

            float xChange = Mathf.PerlinNoise(proportion, 0f) * shakeStrength * curve.Evaluate(proportion) * shakeObjectConstant * Random.Range(-1, 2) * Time.deltaTime;
            float yChange = Mathf.PerlinNoise(0f, proportion) * shakeStrength * curve.Evaluate(proportion) * shakeObjectConstant * Random.Range(-1, 2) * Time.deltaTime;
            objectToShake.transform.localPosition = originalLocalPosition + new Vector3(xChange, yChange, 0);

            yield return null;
        }
        objectToShake.transform.localPosition = originalLocalPosition;
        currentCoroutines.Remove(objectToShake);
    }
}
