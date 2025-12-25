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
            powerCells[i].GetComponent<PowerCell>().CellDropped.AddListener(IndicateReplacementCell);
        }
    }
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
    private void IndicateReplacementCell()
    {
        
    }
}
