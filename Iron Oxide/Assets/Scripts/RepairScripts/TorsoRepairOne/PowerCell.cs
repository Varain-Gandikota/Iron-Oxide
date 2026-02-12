using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PowerCell : MonoBehaviour
{
    private Animator powerCellAnimator;
    private Button powerCellButton;
    private Drag powerCellDrag;

    private UnityEvent replacementCellShown = new UnityEvent();
    private UnityEvent powerCellPlacedIn = new UnityEvent();

    private RectTransform rect;
    private Image spriteImage;

    private bool isInteractable = false;
    private bool isFinished = false;
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
        powerCellDrag = GetComponent<Drag>();
    }
    public void DropCell()
    {
        // play powercell dropping animation here
        if (!isFinished)
            powerCellAnimator.SetTrigger("Drop");
        powerCellButton.enabled = false;
    }
    public void SetColor(Color color)
    {
        spriteImage.color = color;
    }
    public void ReplacementShown()
    {
        replacementCellShown.Invoke();
        powerCellDrag.DoGrab = true;
        SetColor(Color.white);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == transform.parent.gameObject && powerCellDrag.DoGrab)
        {
            isFinished = true;
            powerCellPlacedIn.Invoke();
            IsInteractable = false;
            powerCellAnimator.enabled = true;
            powerCellAnimator.SetTrigger("Replace");
            powerCellDrag.DoGrab = false;
            //ResetPowerCell();
        }

    }
    public void ResetPowerCell()
    {
        isFinished = false;
        IsInteractable = false;
        powerCellDrag.DoGrab = false;
        powerCellAnimator.enabled = true;
        powerCellAnimator.Play("Idle");
        SetColor(Color.white);
    }
}
