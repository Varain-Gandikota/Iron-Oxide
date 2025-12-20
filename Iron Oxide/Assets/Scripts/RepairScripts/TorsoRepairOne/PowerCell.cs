using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PowerCell : MonoBehaviour
{
    private Animator powerCellAnimator;
    private Button powerCellButton;
    private UnityEvent cellDropped = new UnityEvent();
    private bool isInteractable = false;
    public bool IsInteractable { get => isInteractable; 
        set { 
            isInteractable = value;
            transform.GetChild(0).gameObject.SetActive(value);
            powerCellButton.interactable = value;
        }  
    }

    public UnityEvent CellDropped { get => cellDropped; }

    private void Start()
    {
        powerCellAnimator = GetComponent<Animator>();
        powerCellButton = GetComponent<Button>();
    }
    public void DropCell()
    {
        // play powercell dropping animation here
        powerCellAnimator.SetTrigger("Drop");
        CellDropped.Invoke();
    }

}
