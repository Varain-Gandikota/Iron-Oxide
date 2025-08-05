using UnityEngine;
using UnityEngine.InputSystem;

public class RepairManager : MonoBehaviour
{
    [SerializeField] private GameObject repairWheel;
    [SerializeField] private float timeSlowDown = 0.25f;

    [SerializeField] private GameObject repairUIHolder;
    [SerializeField] GameObject[] bodyRepairVisuals = new GameObject[4];
    [SerializeField] GameObject[] headRepairVisuals = new GameObject[4];
    [SerializeField] GameObject[] armRepairVisuals = new GameObject[4];
    [SerializeField] GameObject[] legsRepairVisuals = new GameObject[4];

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
                break;
        }
    }
 
}
public enum RepairChoice
{
    None = -1,
    Head=0,
    Body=1,
    Arms=2, 
    Legs=3
}
