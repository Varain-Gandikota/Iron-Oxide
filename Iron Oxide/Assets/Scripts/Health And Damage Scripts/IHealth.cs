using UnityEngine;
using System;
public abstract class IHealth : MonoBehaviour
{
    private Action onDeath = delegate { };
    private Action<float> onHitPointsChanged = delegate { };
    private float hitPoints;
    private float maxHitPoints;
    public Action<float> OnHitPointsChanged { get => onHitPointsChanged; set => onHitPointsChanged = value; }
    public Action OnDeath { get => onDeath; set => onDeath = value; }
    public float HitPoints
    { 
        get => HitPoints; 
        set
        {
            if (hitPoints != value) { 
                // invokes with the change in hit points
                OnHitPointsChanged.Invoke(value-hitPoints);
            }
            hitPoints = Mathf.Clamp(value, 0, maxHitPoints);
            if (hitPoints <= 0) {
                OnDeath.Invoke();
            }
        }
    }
    public float MaxHitPoints
    {
        get => maxHitPoints;
        set
        {
            maxHitPoints = value;
            // Re-clamp hit points to new max
            HitPoints = hitPoints; 
        }
    }



}
