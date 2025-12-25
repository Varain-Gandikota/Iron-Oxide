using UnityEngine;
using UnityEngine.InputSystem;

public class RepairManager : MonoBehaviour
{
    [SerializeField] private GameObject repairWheel;
    [SerializeField] private float timeSlowDown = 0.25f;

    [SerializeField] private GameObject repairUIHolder;
    [SerializeField] GameObject[] bodyRepairs = new GameObject[4];
    [SerializeField] GameObject[] headRepairs = new GameObject[4];
    [SerializeField] GameObject[] armsRepairs = new GameObject[4];
    [SerializeField] GameObject[] legsRepairs = new GameObject[4];

    [SerializeField] private float bodyDurability = 100;
    [SerializeField] private float headDurability = 100;
    [SerializeField] private float armDurability = 100;
    [SerializeField] private float legsDurability = 100;

    private float maxBodyDurability = 100;
    private float maxHeadDurability = 100;
    private float maxArmDurability = 100;
    private float maxLegsDurability = 100;

    // true when player is actively on the repair screen for any given part
    private bool isRepairing = false;

    // true when player delays/cancels repair 
    private bool delayedRepairing = false;
    private InputAction repairWheelAction;
    private Animator UIHolderAnimator;

    private void OnEnable()
    {
        repairWheelAction = InputSystem.actions.FindAction("Repair Wheel");
        repairWheelAction.performed += ShowRepairWheel;
        repairWheelAction.canceled += ChooseRepairChoice;
    }
    private void Start()
    {
        repairWheel.GetComponent<RepairChoiceWheel>().RepairChosen.AddListener(ShowRepair);
        UIHolderAnimator = repairUIHolder.GetComponent<Animator>();
    }
    public void ShowRepairWheel(InputAction.CallbackContext context)
    {
        UIHolderAnimator.SetTrigger("Come Up");
        Time.timeScale = timeSlowDown;
        repairWheel.SetActive(true);
        UIHolderAnimator.ResetTrigger("Come Up");
    }
    public void ChooseRepairChoice(InputAction.CallbackContext context)
    {
        Time.timeScale = 1;
        repairWheel.SetActive(false);
    }
    public void ShowRepair(RepairChoice choice)
    {
        switch (choice)
        {
            case RepairChoice.None:
                break;
            case RepairChoice.Body:
                UIHolderAnimator.SetTrigger("Come Down");
                int bodyChoice = Random.Range(0, bodyRepairs.Length);
                GameObject torsoRepair = bodyRepairs[bodyChoice];
                torsoRepair.GetComponent<IRepairSystem>().StartMinigame(bodyDurability, bodyDurability/maxBodyDurability);

                break;
        }
    }
 
}
public enum RepairChoice
{
    None = -1,
    Head = 0,
    Body = 1,
    Arms = 2, 
    Legs = 3
}
