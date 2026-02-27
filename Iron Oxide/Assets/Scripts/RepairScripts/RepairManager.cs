using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Events;

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
    private GameObject[] repairGameObjects;
    public float Durability { get; set; }
    public float MaxDurability { get; set; }
    public float RepairAmount { get; set; }
    public IRepairSystem[] RepairOptions { get; set; }
    public GameObject[] RepairGameObjects { get => repairGameObjects; set { repairGameObjects = value; SetRepairGameObjects(value); } }

    public UnityEvent<float, float> OnDurabilityChanged = new();

    public RepairTypeInformation()
    {

    }
    private void SetRepairGameObjects(GameObject[] repairGameObjects)
    {
        RepairOptions = new IRepairSystem[repairGameObjects.Length];
        int i = 0;
        foreach (GameObject g in repairGameObjects)
        {
            RepairOptions[i] = g.GetComponent<IRepairSystem>();
            i++;
        }
    }
    public RepairTypeInformation(float _durability, float _maxDurability, float repairAmount, GameObject[] _repairGameObjects)
    {
        Durability = _durability;
        MaxDurability = _maxDurability;
        RepairAmount = repairAmount;
        RepairOptions = new IRepairSystem[_repairGameObjects.Length];
        int i = 0;
        foreach (GameObject g in _repairGameObjects)
        {
            RepairOptions[i] = g.GetComponent<IRepairSystem>();
            i++;
        }
        repairGameObjects = _repairGameObjects;
        //OnDurabilityChanged.Invoke(Durability, MaxDurability);
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
    public void Repair()
    {
        Durability = Mathf.Clamp(Durability + RepairAmount, 0, MaxDurability);
        OnDurabilityChanged.Invoke(Durability, MaxDurability);
    }

}
public class RepairManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject repairWheel;
    [SerializeField] private float timeSlowDown = 0.25f;

    [SerializeField] private GameObject repairUIHolder;
    [SerializeField] GameObject[] bodyRepairs = { };
    [SerializeField] GameObject[] headRepairs = { };
    [SerializeField] GameObject[] armsRepairs = { };
    [SerializeField] GameObject[] legsRepairs = { };
    //private Dictionary<RepairChoice, RepairTypeInformation> repairs = new();
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
        playerData.repairs[RepairChoice.Head].RepairGameObjects = headRepairs;
        playerData.repairs[RepairChoice.Body].RepairGameObjects = bodyRepairs;
        playerData.repairs[RepairChoice.Arms].RepairGameObjects = armsRepairs;
        playerData.repairs[RepairChoice.Legs].RepairGameObjects = legsRepairs;
        Debug.Log("Body Durability: "+ playerData.repairs[RepairChoice.Body].Durability);
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

        RepairTypeInformation repairInfo = playerData.repairs[choice];

        if (repairInfo.Durability >= repairInfo.MaxDurability || playerData.AmountOfRepairTokens <= 0)
        {
            CloseRepair();
            return;
        }

        isRepairing = true;
        currentRepairChoice = choice;
        Debug.Log(repairInfo.RepairGameObjects);
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
        repairInfo.Repair();
        playerData.AmountOfRepairTokens--;
        CloseRepair();
    }
}
