using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    
    private UnityEvent onTorsoDead = new();
    public UnityEvent OnTorsoDead { get => onTorsoDead; }
}
