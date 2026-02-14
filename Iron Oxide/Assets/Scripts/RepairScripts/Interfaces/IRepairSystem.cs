using UnityEngine;
using UnityEngine.Events;
public interface IRepairSystem
{
    public string Partname { get; }
    public UnityEvent RepairFinished { get; }
    public void StartMinigame(float durability, float durabilityPercentage);
    public void FinishMinigame();
    public void StopMinigame();
}
