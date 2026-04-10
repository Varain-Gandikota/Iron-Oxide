using Mono.Cecil;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TorsoRepairOne : MonoBehaviour, IRepairSystem
{
    private string partName = "Torso";
    private int powerCellsReplaced = 0;
    private byte numberOfCellsToReplace = 0;
    private UnityEvent repairFinished = new UnityEvent();
    public string Partname { get => partName; set => partName = value; }

    public UnityEvent RepairFinished { get => repairFinished; }

    [Header("References")]
    [SerializeField] private GameObject[] powerCells = new GameObject[3];
    private PowerCell[] powerCellsComponents = new PowerCell[3];
    [SerializeField] private Color brokenCellColor;
    private Animator panelAnimator;
    [SerializeField] private GameObject panel;
    private Button panelButton;

    private void Start()
    {
        panelButton = panel.GetComponent<Button>();
        panelAnimator = GetComponent<Animator>();
        panelAnimator.Play("Idle");
        for (int i = 0; i < powerCells.Length; i++)
        {
            //adds the function as a listener
            PowerCell p = powerCells[i].GetComponent<PowerCell>();
            powerCellsComponents[i] = p;
            p.ReplacementCellShown.AddListener(delegate { ReleaseReplacementCell(p); } );
            p.PowerCellPlacedIn.AddListener(delegate { CellReplaced(); });
        }
    }
    // This is required by the interface, so we wait for the panel by using unity events. Once the panel is opened, it starts the rest of the minigame. 
    public void StartMinigame(float durability, float durabilityPercentage)
    {
        panelAnimator.Play("Idle");
        panelButton.enabled = true;
        
        powerCellsReplaced = 0;
        for (int i = 0; i < powerCells.Length; i++)
        {
            PowerCell p = powerCells[i].GetComponent<PowerCell>();
            p.IsInteractable = false;
            p.SetColor(Color.white);
            
        }
        numberOfCellsToReplace = (byte)Mathf.CeilToInt(3 * (1-durabilityPercentage));
        for (byte i = 0; i < numberOfCellsToReplace; i++)
        {
            // Show the white outline to indicate the ones to replace.
            PowerCell p = powerCells[i].GetComponent<PowerCell>();
            p.IsInteractable = true;
            p.SetColor(brokenCellColor);
        }
    }
    public void FinishMinigame()
    {
        StopMinigame();
        repairFinished.Invoke();
    }

    public void StopMinigame()
    {
        panel.transform.SetAsLastSibling();
        foreach (PowerCell p in powerCellsComponents)
        {
            p.ResetPowerCell();
            p.SetColor(Color.white);
        }
        powerCellsReplaced = 0;
        numberOfCellsToReplace = 0;
        panelButton.enabled = false;
        panelAnimator.Play("Close Panel");
    }
    private void ReleaseReplacementCell(PowerCell p)
    {
        p.PowerCellAnimator.enabled = false;
        
    }
    // sets panel as the last sibling so it shows up behind the power cells
    private void SetPanelBehindPowerCells()
    {
        panel.transform.SetAsFirstSibling();
    }
    private void CellReplaced()
    {
        powerCellsReplaced++;
        if (powerCellsReplaced == numberOfCellsToReplace)
        {
            Invoke(nameof(FinishMinigame), 0.3f);

            Debug.Log("Repair Finished");
        }
    }
}
