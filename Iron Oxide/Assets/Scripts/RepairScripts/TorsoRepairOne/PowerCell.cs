using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PowerCell : MonoBehaviour
{
    private Animator powerCellAnimator;
    private Button powerCellButton;
    private UnityEvent cellDropped = new UnityEvent();
    private RectTransform rect;
    private bool isInteractable = false;
    private Image spriteImage;
    public bool IsInteractable { 
        get => isInteractable; 
        set { 
            isInteractable = value;
            rect.GetChild(0).gameObject.SetActive(value);
            powerCellButton.enabled = value;
        }  
    }

    public UnityEvent CellDropped { get => cellDropped; }

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
        CellDropped.Invoke();
    }
    public void SetColor(Color color)
    {
        spriteImage.color = color;
    }
}
