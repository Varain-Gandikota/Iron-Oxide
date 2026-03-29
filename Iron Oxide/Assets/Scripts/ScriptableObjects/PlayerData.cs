using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public Dictionary<RepairChoice, RepairTypeInformation> repairs = new();

    public int amountOfRepairTokens = 3;
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
        AmountOfRepairTokens = 1;
        repairs.Add(RepairChoice.None, new(0, 1, 30, new GameObject[0]));
        repairs.Add(RepairChoice.Head, new(100, 100, 30, new GameObject[0]));
        repairs.Add(RepairChoice.Body, new(10, 100, 30, new GameObject[0]));
        repairs.Add(RepairChoice.Arms, new(100, 100, 30, new GameObject[0]));
        repairs.Add(RepairChoice.Legs, new(100, 100, 45, new GameObject[0]));
    }
    public void MakeInvulnerable(bool isInvulnerable)
    {
        for (int i = 0; i < repairs.Values.Count; i++) {
            repairs.Values.ElementAt(i).Invulnerable = isInvulnerable;
        }
    }

}
