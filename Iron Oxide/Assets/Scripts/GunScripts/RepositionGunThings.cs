using UnityEngine;

public class RepositionGunThings : MonoBehaviour
{
    public GameObject objectsToReposition;
    public Vector2 newPositions;

    public void Reposition()
    {
        objectsToReposition.transform.localPosition = newPositions;
    }
}
