using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 8f;
    private bool isGrounded = true;
    [SerializeField] Rigidbody2D rb;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input. GetAxis("Horizontal");
        transform.Translate(move * speed * Time.deltaTime, 0, 0);


        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void SayHello()
    {
        Debug.Log("woi manusia");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}