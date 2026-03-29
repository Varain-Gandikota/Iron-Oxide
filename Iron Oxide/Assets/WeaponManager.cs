using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject[] currentWeapons = new GameObject[9];
    private GameObject equippedWeapon;
    private int parryLayer = 0;

    private void Awake()
    {
        parryLayer = LayerMask.NameToLayer("Parry");
    }
    public void ActivateCurrentWeapon()
    {
        if (!equippedWeapon)
            return;
    }
    public void AddWeapon()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == parryLayer)
            Debug.Log("Parried Detected With " + collision.gameObject.name);

    }
}
