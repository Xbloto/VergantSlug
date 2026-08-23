using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 20;
    public float jumpForce = 30f;
    public bool isGrounded = true;
    public bool canDoubleJump = false;
    [SerializeField] Rigidbody2D rb;

    private PlayerVergant playerAudio;

    void Start()
    {
        playerAudio = GetComponent<PlayerVergant>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        transform.Translate(move * speed * Time.deltaTime, 0, 0);

        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
                canDoubleJump = true;

                if (playerAudio != null)
                {
                    playerAudio.isGrounded = false;
                    playerAudio.PlayJumpSound();
                }
            }
            else if (canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = false;

                if (playerAudio != null)
                {
                    playerAudio.isGrounded = false;
                    playerAudio.PlayJumpSound();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;

            if (playerAudio != null)
            {
                playerAudio.isGrounded = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;

            if (playerAudio != null)
            {
                playerAudio.isGrounded = false;
            }
        }
    }
}