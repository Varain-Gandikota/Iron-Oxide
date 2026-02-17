using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Transactions;

public class RepairChargeManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
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

    private void Update()
    {
        ChargeAmount += autoChargeAmount* Time.deltaTime;
    }
    private void OnEnable()
    {
        playerData.OnAmountOfRepairTokensChanged.AddListener(UpdateAmountOfTokens);
    }
    void UpdateAmountOfTokens() {
        AmountOfTokens = playerData.AmountOfRepairTokens;
    }
     private void OnDisable()
    {
        playerData.OnAmountOfRepairTokensChanged.RemoveListener(UpdateAmountOfTokens);
    }

}
