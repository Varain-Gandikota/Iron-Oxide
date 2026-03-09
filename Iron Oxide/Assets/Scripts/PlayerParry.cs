using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private Animator gunHolderAnimator;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float timeUntilReturnToIdle = 0.1f;
    [SerializeField] private bool canParry = true;

    private bool returnToNormal = true;
    private bool parryLeftOrRight = false;



}
