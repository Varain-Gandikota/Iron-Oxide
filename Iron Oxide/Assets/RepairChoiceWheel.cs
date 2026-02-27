using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

public class RepairChoiceWheel : MonoBehaviour
{
    
    [SerializeField] private GameObject[] repairChoices = new GameObject[4];
    [SerializeField] private UnityEvent<RepairChoice> repairChosen = new UnityEvent<RepairChoice>();
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float scaleMultiplier = 0.8f;
    private RepairChoice repairChoice = RepairChoice.None;
    private RepairChoice hoveredChoice = RepairChoice.None;
    private InputAction mousePositionAction;
    private LayerMask repairWheelMask;
    public UnityEvent<RepairChoice> RepairChosen { get => repairChosen; set => repairChosen = value; }

    private void OnEnable()
    {
        mousePositionAction = InputSystem.actions.FindAction("Look");
    }
    private void Start()
    {
        repairWheelMask = LayerMask.GetMask("Repair Wheel");
    }
    private void FixedUpdate()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(mousePositionAction.ReadValue<Vector2>()) - transform.position;
        RaycastHit2D result = Physics2D.Raycast(transform.position, direction, 1000f, repairWheelMask);
        Debug.DrawRay(transform.position, direction, Color.white, 4);
        if (result)
        {
            GameObject highlightedChoice = result.transform.gameObject;
            for (int i = 0; i < repairChoices.Length; i++)
            {
                GameObject choice = repairChoices[i];
                IndicateChoice(choice, choice.Equals(highlightedChoice), i);
            }
        }
    }
    private void OnDisable()
    {
        repairChoice = hoveredChoice;
        repairChosen.Invoke(repairChoice);
        for (int i = 0; i < repairChoices.Length; i++)
        {
            GameObject choice = repairChoices[i];
            IndicateChoice(choice, false, i);
        }
    }
    private void IndicateChoice(GameObject choice, bool active, int index = 4)
    {
        RepairTypeInformation repairInfo = playerData.repairs[(RepairChoice)index];
        if (repairInfo.Durability >= repairInfo.MaxDurability || playerData.AmountOfRepairTokens <= 0)
        {
            choice.transform.GetChild(0).gameObject.SetActive(false);
        } else
            choice.transform.GetChild(0).gameObject.SetActive(active);
        if (active) {

            hoveredChoice = (RepairChoice)index;
        }

    }

}
