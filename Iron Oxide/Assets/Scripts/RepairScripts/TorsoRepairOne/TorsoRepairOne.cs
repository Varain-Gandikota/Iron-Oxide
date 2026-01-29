using UnityEngine;
using UnityEngine.Events;

public class TorsoRepairOne : MonoBehaviour, IRepairSystem
{
    private string partName = "Torso";
    private bool isComplete = false;
    private int powerCellsReplaced = 0;

    public string Partname { get => partName; set => partName = value; }
    public bool IsComplete { get => isComplete; set => isComplete = value; }

    [SerializeField] private GameObject[] powerCells = new GameObject[3];
    [SerializeField] private Color brokenCellColor;
    public void Repair()
    {
        throw new System.NotImplementedException();
    }
    private void Start()
    {
        for (int i = 0; i < powerCells.Length; i++)
        {
            //adds the function as a listener
            PowerCell p = powerCells[i].GetComponent<PowerCell>();
            p.ReplacementCellShown.AddListener(delegate { ReleaseReplacementCell(p); } );
        }
    }
    private void StartReplaceCellMinigame(float durability, float durabilityPercentage)
    {

    }
    // This is required by the interface, so we wait for the panel by using unity events. Once the panel is opened, it starts the rest of the minigame. 
    public void StartMinigame(float durability, float durabilityPercentage)
    {
        powerCellsReplaced = 0;
        for (int i = 0; i < powerCells.Length; i++)
        {
            PowerCell p = powerCells[i].GetComponent<PowerCell>();
            p.IsInteractable = false;
            p.SetColor(Color.white);
            
        }
        byte number_broken = (byte)Mathf.CeilToInt(3 * (1-durabilityPercentage));
        for (byte i = 0; i < number_broken; i++)
        {
            // Show the white outline to indicate the ones to replace.
            PowerCell p = powerCells[i].GetComponent<PowerCell>();
            p.IsInteractable = true;
            p.SetColor(brokenCellColor);
        }
    }

    public void StopMinigame()
    {
        throw new System.NotImplementedException();
    }

    public void InterruptMinigame()
    {
        throw new System.NotImplementedException();
    }
    private void ReleaseReplacementCell(PowerCell p)
    {
        p.PowerCellAnimator.enabled = false; 
    }
}
