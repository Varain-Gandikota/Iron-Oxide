using UnityEngine;

public class DamageManager : MonoBehaviour
{

    public static void ApplyDamage(GameObject target, float damageAmount)
    {
        if (target.TryGetComponent(out IHealth health))
        {
            health.HitPoints -= damageAmount;
        }
    }
}
