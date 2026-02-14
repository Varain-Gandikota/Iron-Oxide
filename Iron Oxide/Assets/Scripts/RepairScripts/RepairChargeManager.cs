using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Transactions;

public class RepairChargeManager : MonoBehaviour
{
    public static RepairChargeManager Instance;
    [SerializeField] private Image repairChargeBar;
    [SerializeField] private TextMeshProUGUI tokenAmountText;
    [SerializeField] private int maxChargeAmount;
    [SerializeField] private int maxAmountTokens;
    [SerializeField] private float autoChargeAmount; 
    private float chargeAmount = 0;
    private int amountOfTokens = 1;
    private bool doAutoCharge = true;
    public float ChargeAmount { 
        get => chargeAmount; 
        set {
            chargeAmount = value;
            repairChargeBar.fillAmount = chargeAmount / maxChargeAmount;
            if (chargeAmount >= maxChargeAmount && amountOfTokens < maxAmountTokens) {
                AmountOfTokens++;
                chargeAmount = 0;
            }
        }
    }

    public int AmountOfTokens { 
        get => amountOfTokens; 
        set {
            int oldValue = amountOfTokens;
            amountOfTokens = Mathf.Clamp(value, 0, maxAmountTokens);
            tokenAmountText.text = amountOfTokens.ToString();
        }  
    }

    public bool DoAutoCharge { get => doAutoCharge; set => doAutoCharge = value; }

    private void OnEnable()
    {
        Instance = this;
        ChargeAmount = 0;
        AmountOfTokens = 3;
    }

    private void Update()
    {
        ChargeAmount += autoChargeAmount* Time.deltaTime;
    }


}
