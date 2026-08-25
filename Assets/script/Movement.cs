using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpForce = 34f;
    public bool isGrounded = true;
    public bool canDoubleJump = false;
    
    public Animator animator;
    [SerializeField] private Rigidbody2D rb;

    private PlayerVergant playerAudio;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerVergant>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        animator.SetFloat("speed", Mathf.Abs(moveInput));

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        animator.SetBool("IsJumping", true);

        if (playerAudio != null)
        {
            playerAudio.isGrounded = false;
            playerAudio.PlayJumpSound();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            canDoubleJump = false;

            if (playerAudio != null)
            {
                playerAudio.isGrounded = true;
            }
        }
    }
}