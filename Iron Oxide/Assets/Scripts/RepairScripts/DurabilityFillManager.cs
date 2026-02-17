using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityFillManager : MonoBehaviour
{
    [SerializeField] private Image durabilityFillImage;
    [SerializeField] private Image ghostDurabilityFillImage;
    [SerializeField] private float fillChangeDuration = 0.3f;
    [SerializeField] private float colorChangeDuration = 0.1f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private RepairChoice repairChoice;
    [SerializeField] private PlayerData playerData;

    private float time = 0f;
    private float ogBackFill = 0f;

    private Coroutine curCor = null;
    private bool goUpToFront = false;
    public void SetDurabilityFill(float durability, float maxDurability)
    {
        time = 0f;
        float durabilityPercentage = durability / maxDurability;
        if (durabilityFillImage.fillAmount > durabilityPercentage)
        {
            if (curCor != null)
                StopCoroutine(curCor);
            curCor = StartCoroutine(ShakeBar(Mathf.Abs(durabilityFillImage.fillAmount - durabilityPercentage)));
            goUpToFront = true;
            ogBackFill = ghostDurabilityFillImage.fillAmount;
            durabilityFillImage.fillAmount = durabilityPercentage;
            durabilityFillImage.color = Color.red; 
            StartCoroutine(ColorToWhite(Color.red));
            
        }
        else if (durabilityFillImage.fillAmount < durabilityPercentage)
        {
            goUpToFront = false;
            ogBackFill = durabilityFillImage.fillAmount;
            ghostDurabilityFillImage.fillAmount = durabilityPercentage;
            durabilityFillImage.color = Color.green; 
            StartCoroutine(ColorToWhite(Color.green)); 
        }
        
    }
    
    private IEnumerator ShakeBar(float distanceFromNewValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            float shakeAmount = Mathf.Sin(elapsedTime * 50) * distanceFromNewValue * shakeStrength; // Adjust the shake strength as needed
            durabilityFillImage.rectTransform.localPosition = new Vector3(shakeAmount, durabilityFillImage.rectTransform.localPosition.y, durabilityFillImage.rectTransform.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        durabilityFillImage.rectTransform.localPosition = new Vector3(0, durabilityFillImage.rectTransform.localPosition.y, durabilityFillImage.rectTransform.localPosition.z); // Reset position after shaking
    }
    private void OnEnable()
    {
        time = 0f;
    }
    private void Start()
    {
        playerData.repairs[repairChoice].OnDurabilityChanged.AddListener(SetDurabilityFill);
        SetDurabilityFill(playerData.repairs[repairChoice].Durability, playerData.repairs[repairChoice].MaxDurability);
    }
    private IEnumerator ColorToWhite(Color colorToFadeTo)
    {
        float elapsedTime = 0f;
        Color startColor = colorToFadeTo;
        Color endColor = Color.white;
        while (elapsedTime < colorChangeDuration)
        {
            durabilityFillImage.color = Color.Lerp(startColor, endColor, elapsedTime / colorChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        durabilityFillImage.color = endColor; // Ensure the final color is set to white
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetDurabilityFill(Random.Range(0f, 1f), 1);
        }

        if (durabilityFillImage.fillAmount == ghostDurabilityFillImage.fillAmount)
            return;
        time += Time.deltaTime;
        float proportion = Mathf.Pow(time / fillChangeDuration, 4);
        if (goUpToFront)
        {
            ghostDurabilityFillImage.fillAmount = Mathf.Lerp(ogBackFill, durabilityFillImage.fillAmount, proportion);
        }
        else
        {
            durabilityFillImage.fillAmount = Mathf.Lerp(ogBackFill, ghostDurabilityFillImage.fillAmount, proportion);
        }

    }
}
