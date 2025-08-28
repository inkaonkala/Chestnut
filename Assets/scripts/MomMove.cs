using UnityEngine;
using UnityEngine.InputSystem;

public class MomMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpForce = 20f;
    public Transform groundCheck;
    public LayerMask groundLayer; private Rigidbody2D body;

    private Vector2 movement;
    private MomInput input;

    private bool isGrounded;

    private void Awake()
    {
        input = new MomInput();
    }

    private void OnEnable()
    {
        input.Mom.Enable();
        input.Mom.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Mom.Move.canceled += ctx => movement = Vector2.zero;
        input.Mom.Jump.performed += ctx => TryJump();
    }

    private void OnDisable()
    {
        input.Mom.Disable();
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        body.linearVelocity = new Vector2(movement.x * moveSpeed, body.linearVelocity.y);

    //    Debug.Log("Is Grounded: " + isGrounded);
        //body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    private void TryJump()
    {
        if (isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
