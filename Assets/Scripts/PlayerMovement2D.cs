using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float rayDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioSource audioSource;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAnimation();
        HandleFlip();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            rayDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (animator != null)
                animator.SetTrigger("JumpTrigger");

            if (audioSource != null && jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }
    }

    private void HandleAnimation()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void HandleFlip()
    {
        if (spriteRenderer == null) return;

        if (rb.linearVelocity.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.1f)
            spriteRenderer.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            groundCheck.position,
            groundCheck.position + Vector3.down * rayDistance
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatformTag"))
            transform.SetParent(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatformTag"))
            transform.SetParent(null);
    }
}