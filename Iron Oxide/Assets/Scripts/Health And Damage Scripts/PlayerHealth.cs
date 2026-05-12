using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private PlayerData playerData;

    public void Damage(float damageAmount)
    {
        int indexToDamage = Random.Range(0, playerData.repairs.Count);
        playerData.repairs[(RepairChoice)indexToDamage].Damage(damageAmount);
    }

    public void Heal(float healAmount)
    {

    }
}
