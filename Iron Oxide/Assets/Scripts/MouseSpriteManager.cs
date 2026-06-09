using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CursorType
{
    HandPoint = 0,
    HandReach = 1,
    HandGrab = 2,
    Screwdriver = 3
}
public class MouseSpriteManager : MonoBehaviour
{
    private static MouseSpriteManager instance;

    [SerializeField] private Image handImage;
    private CursorType currentCursorType = CursorType.HandPoint;
    private Dictionary<CursorType, Sprite> mouseSprites;

    public static MouseSpriteManager Instance { get => instance; }

    private void Start()
    {
        instance = this;
        mouseSprites = new Dictionary<CursorType, Sprite>();
        mouseSprites.Add(CursorType.HandPoint, Resources.Load<Sprite>("CustomCursors/HandPoint"));
        mouseSprites.Add(CursorType.HandReach, Resources.Load<Sprite>("CustomCursors/HandReach"));
        mouseSprites.Add(CursorType.HandGrab, Resources.Load<Sprite>("CustomCursors/HandGrab"));
        mouseSprites.Add(CursorType.Screwdriver, Resources.Load<Sprite>("CustomCursors/Screwdriver"));
        handImage.sprite = mouseSprites[CursorType.HandPoint];
    }
    public void ChangeCursor(CursorType newCursorType)
    {
        if (currentCursorType == newCursorType) return;
        handImage.sprite = mouseSprites[newCursorType];
        currentCursorType = newCursorType;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HandReachGrab"))
        {
            ChangeCursor(CursorType.HandReach);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ChangeCursor(CursorType.HandPoint);
    }
}
