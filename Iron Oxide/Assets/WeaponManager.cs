using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] currentWeapons = new GameObject[9];
    private GameObject equippedWeapon;

    public void ActivateCurrentWeapon()
    {
        if (!equippedWeapon)
            return;
    }
    public void AddWeapon()
    {
        
    }
}