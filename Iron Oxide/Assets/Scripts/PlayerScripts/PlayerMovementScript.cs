using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject legManager;
    [SerializeField] private GameObject torso;
    private bool canMove = true;
    private Vector2 moveValue;

    private Animator animator;
    private Rigidbody2D rb2D;

    private InputAction lookAction;
    private InputAction jumpAction;


    public bool CanMove { get => canMove; set => canMove = value; }

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.speed = 0;
    }
    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
        if (moveValue != Vector2.zero)
        {
            animator.speed = 1;
            animator.Play("Walk");
            legManager.transform.up = moveValue;
        }
        else
            animator.speed = 0;
    }
    private void FixedUpdate()
    {
        if (canMove)
            rb2D.AddForce(movementSpeed * Time.fixedDeltaTime * moveValue);

        Vector2 direction = (Camera.main.ScreenToWorldPoint(lookAction.ReadValue<Vector2>()) - transform.position).normalized;
        torso.transform.up = direction;
    }

}
