using UnityEngine;
using UnityEngine.Events;

public class TorsoRepairOne : MonoBehaviour, IRepairSystem
{
    private string partName = "Torso";
    private bool isComplete = false;

    public string Partname { get => partName; set => partName = value; }
    public bool IsComplete { get => isComplete; set => isComplete = value; }

    [SerializeField] private GameObject[] powerCells = new GameObject[3];
    private int powerCellsReplaced = 0;
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
            powerCells[i].GetComponent<PowerCell>().IsInteractable = false;
        }
    }
    public void StartMinigame(double durability, double durabilityPercentage)
    {
        powerCellsReplaced = 0;
        for (int i = 0; i < powerCells.Length; i++)
        {
            powerCells[i].GetComponent<PowerCell>().IsInteractable = false;
        }
        byte number_broken = (byte)(3 * durabilityPercentage);
        for (byte i = 0; i < number_broken; i++)
        {
            // Show the white outline to indicate the ones to replace.
            powerCells[i].GetComponent<PowerCell>().IsInteractable = true;

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
