using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PowerCell : MonoBehaviour
{
    private Animator powerCellAnimator;
    private Button powerCellButton;

    private UnityEvent replacementCellShown = new UnityEvent();
    private UnityEvent powerCellPlacedIn = new UnityEvent();

    private RectTransform rect;
    private Image spriteImage;

    private bool isInteractable = false;
    public bool IsInteractable { 
        get => isInteractable; 
        set { 
            isInteractable = value;
            rect.GetChild(0).gameObject.SetActive(value);
            powerCellButton.enabled = value;
        }  
    }

    public UnityEvent ReplacementCellShown { get => replacementCellShown; }
    public Animator PowerCellAnimator { get => powerCellAnimator; }
    public UnityEvent PowerCellPlacedIn { get => powerCellPlacedIn;}

    private void Start()
    {
        powerCellAnimator = GetComponent<Animator>();
        powerCellButton = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        spriteImage = rect.GetChild(1).gameObject.GetComponent<Image>();
        IsInteractable = false;
    }
    public void DropCell()
    {
        // play powercell dropping animation here
        powerCellAnimator.SetTrigger("Drop");
    }
    public void SetColor(Color color)
    {
        spriteImage.color = color;
    }
    public void ReplacementShown()
    {
        replacementCellShown.Invoke();
        SetColor(Color.white);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == transform.parent.gameObject && IsInteractable)
        {
            Debug.Log("Power cell placed in");
        }

    }
}
