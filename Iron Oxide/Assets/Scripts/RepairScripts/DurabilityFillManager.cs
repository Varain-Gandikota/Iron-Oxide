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

    private RectTransform durabilityRectTransform;
    private float time = 0f;
    private float ogBackFill = 0f;

    private Coroutine curCor = null;
    private bool goUpToFront = false;
    private Color ogColor = Color.white;
    private Color endColor = Color.white;
    private bool durabilityLow = false;

    public void SetDurabilityFill(float durability, float maxDurability)
    {

        time = 0f;
        float durabilityPercentage = durability / maxDurability;
        durabilityLow = durabilityPercentage <= 0.3f;
        endColor = durabilityLow ? Color.red : ogColor;
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
            durabilityRectTransform.localPosition = new Vector3(shakeAmount, durabilityRectTransform.localPosition.y, durabilityRectTransform.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        durabilityRectTransform.localPosition = new Vector3(0, durabilityRectTransform.localPosition.y, durabilityRectTransform.localPosition.z); // Reset position after shaking
    }
    private void OnEnable()
    {
        time = 0f;
    }
    private void Start()
    {
        durabilityRectTransform = durabilityFillImage.rectTransform;
        ogColor = durabilityFillImage.color;
        playerData.repairs[repairChoice].OnDurabilityChanged.AddListener(SetDurabilityFill);
        SetDurabilityFill(playerData.repairs[repairChoice].Durability, playerData.repairs[repairChoice].MaxDurability);
        
        
    }
    private IEnumerator ColorToWhite(Color colorToFadeTo)
    {
        float elapsedTime = 0f;
        Color startColor = colorToFadeTo;
        while (elapsedTime < colorChangeDuration)
        {
            durabilityFillImage.color = Color.Lerp(startColor, endColor, elapsedTime / colorChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        durabilityFillImage.color = endColor; 
    }
    void Update()
    {
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
