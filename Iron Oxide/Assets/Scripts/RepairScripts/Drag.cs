using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Button))]
public class Drag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool lockXPosition;
    [SerializeField] private bool lockYPosition;
    [SerializeField] private bool doGrab = false;
    private bool mouseHeld = false;
    private RectTransform rectTransform;
    public bool DoGrab { get => doGrab; set => doGrab = value; }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (mouseHeld)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rectTransform.position = new Vector2(lockXPosition ? rectTransform.position.x : mousePosition.x, lockYPosition ? rectTransform.position.y : mousePosition.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseHeld = doGrab;
        Debug.Log("Recieved Pointer Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseHeld = false;
        Debug.Log("Recieved Pointer Up");
    }
}
