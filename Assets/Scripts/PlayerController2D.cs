using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public bool topDown = false;
    public float speed = 6f;
    public float jumpForce = 12f;

    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
    Vector2 input;
    bool grounded;
    bool jumpPressed;

    Vector2 Velocity
    {
#if UNITY_6000_0_OR_NEWER
        get => rb.linearVelocity;
        set => rb.linearVelocity = value;
#else
        get => rb.velocity;
        set => rb.velocity = value;
#endif
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        if (topDown) rb.gravityScale = 0f;
    }

    void ReadInput(out Vector2 move, out bool jump)
    {
#if ENABLE_INPUT_SYSTEM
        move = Vector2.zero;
        jump = false;
        Keyboard k = Keyboard.current;
        if (k == null) return;
        move.x = (k.dKey.isPressed || k.rightArrowKey.isPressed ? 1f : 0f)
               - (k.aKey.isPressed || k.leftArrowKey.isPressed ? 1f : 0f);
        move.y = (k.wKey.isPressed || k.upArrowKey.isPressed ? 1f : 0f)
               - (k.sKey.isPressed || k.downArrowKey.isPressed ? 1f : 0f);
        jump = k.spaceKey.wasPressedThisFrame;
#else
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jump = Input.GetButtonDown("Jump");
#endif
    }

    void Update()
    {
        ReadInput(out Vector2 move, out bool jump);

        input.x = move.x;
        input.y = topDown ? move.y : 0f;
        if (input.sqrMagnitude > 1f) input.Normalize();

        if (!topDown)
        {
            grounded = groundCheck != null &&
                       Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
            if (jump && grounded) jumpPressed = true;
        }

        if (spriteRenderer != null && input.x != 0f)
            spriteRenderer.flipX = input.x < 0f;

        if (animator != null)
        {
            animator.SetFloat("Speed", input.magnitude);
            animator.SetBool("Grounded", topDown || grounded);
            animator.SetFloat("YVelocity", Velocity.y);
        }
    }

    void FixedUpdate()
    {
        if (topDown)
        {
            Velocity = input * speed;
            return;
        }

        Velocity = new Vector2(input.x * speed, Velocity.y);

        if (jumpPressed)
        {
            Velocity = new Vector2(Velocity.x, jumpForce);
            jumpPressed = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}