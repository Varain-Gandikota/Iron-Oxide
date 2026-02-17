using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public Dictionary<RepairChoice, RepairTypeInformation> repairs = new();

    private int amountOfRepairTokens = 3;
    public int maxAmountOfRepairTokens = 3;
    public UnityEvent OnAmountOfRepairTokensChanged = new();

    public int AmountOfRepairTokens { 
        get => amountOfRepairTokens; 
        set {
            amountOfRepairTokens = Mathf.Clamp(value, 0, maxAmountOfRepairTokens);
            OnAmountOfRepairTokensChanged.Invoke();
        }
    }

    void OnEnable()
    {
        AmountOfRepairTokens = 3;
        repairs.Add(RepairChoice.None, new());
        repairs.Add(RepairChoice.Head, new(100, 100, 30, new GameObject[0]));
        repairs.Add(RepairChoice.Body, new(30, 100, 50, new GameObject[0]));
        repairs.Add(RepairChoice.Arms, new(100, 100, 30, new GameObject[0]));
        repairs.Add(RepairChoice.Legs, new(100, 100, 45, new GameObject[0]));
    }

}
