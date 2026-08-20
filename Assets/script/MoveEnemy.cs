using UnityEngine;
using System.Collections; 
public class MoveEnemy : MonoBehaviour
{
    [Header("Pengaturan Gerak")]
    public float speed = 2f;           
    public float patrolDistance = 3f;  

    [Header("Pengaturan Nyawa & Efek")]
    public int maxHealth = 3;          
    public Color hitColor = Color.red; 
    public float hitDuration = 0.1f;   

    private float startX;
    private bool movingRight = true;
    private Rigidbody2D rb;
    
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        
        originalColor = spriteRenderer.color; 
        currentHealth = maxHealth;           
        startX = transform.position.x; 
    }

    void Update()
    {
        if (transform.position.x > startX + patrolDistance)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x < startX - patrolDistance)
        {
            movingRight = true;
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentHealth--; 
        
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = hitColor;            
        yield return new WaitForSeconds(hitDuration); 
        spriteRenderer.color = originalColor;       
    }

    void Die()
    {
        Destroy(gameObject); 
    }
}