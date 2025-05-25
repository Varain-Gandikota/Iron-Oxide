using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private bool canMove = true;
    private Vector2 moveValue;

    [SerializeField] private Animator animator;
    private Rigidbody2D rb2D;

    private InputAction lookAction;
    private InputAction jumpAction;


    public bool CanMove { get => canMove; set => canMove = value; }

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        rb2D = GetComponent<Rigidbody2D>();
        moveValue = Vector2.zero;
    }
    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
        if (moveValue.Equals(Vector2.zero))
            animator.speed = 0;
        else
        {
            animator.speed = 1;
            animator.SetFloat("X", moveValue.x);
            animator.SetFloat("Y", moveValue.y);
        }
    }
    private void FixedUpdate()
    {
        if (canMove)
            rb2D.AddForce(movementSpeed * Time.fixedDeltaTime * moveValue);
    }

}
