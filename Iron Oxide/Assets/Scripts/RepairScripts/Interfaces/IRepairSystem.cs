using UnityEngine;
public interface IRepairSystem
{
    public string Partname { get; }
    public bool IsComplete { get; }
    public void Repair();
    public void StartMinigame(double durability, double durabilityPercentage);
    public void StopMinigame();
    public void InterruptMinigame();
}
