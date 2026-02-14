using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;

public enum RepairChoice
{
    None = 4,
    Head = 0,
    Body = 1,
    Arms = 2,
    Legs = 3
}
public class RepairTypeInformation
{
    public float Durability { get; set; }
    public float MaxDurability { get; set; }
    public float RepairAmount { get; set; }
    public IRepairSystem[] RepairOptions { get; set; }
    public GameObject[] RepairGameObjects { get; set; }

    public RepairTypeInformation()
    {

    }
    public RepairTypeInformation(float _durability, float _maxDurability, float repairAmount, GameObject[] repairGameObjects)
    {
        Durability = _durability;
        MaxDurability = _maxDurability;
        RepairAmount = repairAmount;
        RepairOptions = new IRepairSystem[repairGameObjects.Length];
        int i = 0;
        foreach (GameObject g in repairGameObjects)
        {
            RepairOptions[i] = g.GetComponent<IRepairSystem>();
            i++;
        }
        RepairGameObjects = repairGameObjects;
    }

    // returns a random repair of the struct repair type, disabling all other options. 
    public IRepairSystem ReturnRandomRepair()
    {
        int index = Random.Range(0, RepairOptions.Length);
        for (int i = 0; i < RepairOptions.Length; i++)
        {
            if (RepairGameObjects[i])
                RepairGameObjects[i].SetActive(i == index);
        }
        return RepairOptions[index];

    }

}
public class RepairManager : MonoBehaviour
{
    [SerializeField] private GameObject repairWheel;
    [SerializeField] private float timeSlowDown = 0.25f;

    [SerializeField] private GameObject repairUIHolder;
    [SerializeField] GameObject[] bodyRepairs = { };
    [SerializeField] GameObject[] headRepairs = { };
    [SerializeField] GameObject[] armsRepairs = { };
    [SerializeField] GameObject[] legsRepairs = { };
    private Dictionary<RepairChoice, RepairTypeInformation> repairs = new();
    // true when player is actively on the repair screen for any given part
    private bool isRepairing = false;

    // true when player delays/cancels repair 
    private bool delayedRepairing = false;
    private InputAction repairWheelAction;
    private Animator UIHolderAnimator;
    private RepairChoice currentRepairChoice = RepairChoice.None;
    private IRepairSystem currentRepairSystem;

    private void OnEnable()
    {
        repairWheelAction = InputSystem.actions.FindAction("Repair Wheel");
        repairWheelAction.performed += ShowRepairWheel;
        repairWheelAction.canceled += ChooseRepairChoice;
        currentRepairChoice = RepairChoice.None;
    }
    private void Start()
    {
        repairWheel.GetComponent<RepairChoiceWheel>().RepairChosen.AddListener(ShowRepair);
        UIHolderAnimator = repairUIHolder.GetComponent<Animator>();
        repairs.Add(RepairChoice.None, new());
        repairs.Add(RepairChoice.Head, new(100, 100, 30, headRepairs));
        repairs.Add(RepairChoice.Body, new(30, 100, 50, bodyRepairs));
        repairs.Add(RepairChoice.Arms, new(100, 100, 30, armsRepairs));
        repairs.Add(RepairChoice.Legs, new(100, 100, 45, legsRepairs));
        Debug.Log("Body Durability: "+repairs[RepairChoice.Body].Durability);
    }
    public void ShowRepairWheel(InputAction.CallbackContext context)
    {
        UIHolderAnimator.SetTrigger("Come Up");
        repairWheel.SetActive(true);
        Time.timeScale = timeSlowDown;
        UIHolderAnimator.ResetTrigger("Come Up");
    }
    public void ChooseRepairChoice(InputAction.CallbackContext context)
    {
        Time.timeScale = 1;
        repairWheel.SetActive(false);
    }
    public void ShowRepair(RepairChoice choice)
    {
        Debug.Log("Choice: " + choice);
        if (currentRepairChoice == choice) return;
        if (choice == RepairChoice.None)
        {
            CloseRepair();
            return;
        }

        RepairTypeInformation repairInfo = repairs[choice];

        if (repairInfo.Durability >= repairInfo.MaxDurability)
        {
            CloseRepair();
            return;
        }

        isRepairing = true;
        currentRepairChoice = choice;
        IRepairSystem repairSystem = repairInfo.ReturnRandomRepair();
        currentRepairSystem = repairSystem;
        repairSystem.RepairFinished.AddListener(delegate { FinishRepair(ref repairInfo); });
        UIHolderAnimator.SetTrigger("Come Down");
        repairSystem.StartMinigame(repairInfo.Durability, repairInfo.Durability / repairInfo.MaxDurability);
    }
    public void CloseRepair()
    {
        UIHolderAnimator.SetTrigger("Come Up");
        isRepairing = false;
        currentRepairChoice = RepairChoice.None;
        if (currentRepairSystem != null)
        {
            currentRepairSystem.RepairFinished.RemoveAllListeners();
            currentRepairSystem.StopMinigame();
        }
    }
    public void FinishRepair(ref RepairTypeInformation repairInfo)
    {
        repairInfo.Durability = Mathf.Clamp(repairInfo.Durability + repairInfo.RepairAmount, 0, repairInfo.MaxDurability);
        RepairChargeManager.Instance.AmountOfTokens--;
        CloseRepair();
    }
}
