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
    private Camera mainCamera = null;
    private RectTransform rectTransform;
    private Vector3 cachedPosition;
    public bool DoGrab { get => doGrab; set => doGrab = value; }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (mouseHeld)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            cachedPosition = rectTransform.position;
            rectTransform.position = new Vector2(
                lockXPosition ? cachedPosition.x : mousePosition.x, 
                lockYPosition ? cachedPosition.y : mousePosition.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseHeld = doGrab;
        //Debug.Log("Recieved Pointer Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseHeld = false;
        //Debug.Log("Recieved Pointer Up");
    }
}
