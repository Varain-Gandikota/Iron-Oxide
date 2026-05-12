using UnityEngine;

public class ParryableScript : MonoBehaviour
{
    [SerializeField] private GameObject associatedHitBox;


    public void GetParried()
    {
        if (associatedHitBox.TryGetComponent(out HitBox parryable))
        {
            parryable.GetParried();
        }
    }
}
