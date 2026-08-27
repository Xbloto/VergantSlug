using UnityEngine;
using System.Collections; 

public class Jamur : MonoBehaviour
{
    [Header("Pengaturan Gerak & Patroli")]
    public float speed = 2f;           
    public float patrolDistance = 3f;  

    [Header("Pengaturan Deteksi & Kejar Player")]
    public float chaseSpeed = 3.5f;     
    public float detectionRange = 5f;   
    public float stoppingDistance = 2f; 

    [Header("Pengaturan Menembak Enemy")]
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float fireRate = 1f;
    public AudioSource audioSource;
    public AudioClip shootSound;
    public float minPitch = 0.85f;
    public float maxPitch = 1.15f;

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
    
    private Transform playerTransform;
    private float nextFireTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        originalColor = spriteRenderer.color; 
        currentHealth = maxHealth;           
        startX = transform.position.x;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        FindPlayer(); // Coba cari player di awal
    }

    // Fungsi khusus buat nyari player terus-terusan kalau hilang
    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            Debug.Log("<color=green>Mantap, Jamur berhasil menemukan Player!</color>");
        }
    }

    void Update()
    {
        // KALAU PLAYER BELUM KETEMU, CARI TERUS!
        if (playerTransform == null)
        {
            FindPlayer();
            return; // Jangan jalankan kode nembak kalau player belum ada
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (Time.time >= nextFireTime)
            {
                ShootAtPlayer();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            if (transform.position.x > startX + patrolDistance && movingRight)
            {
                movingRight = false;
                Flip();
            }
            else if (transform.position.x < startX - patrolDistance && !movingRight)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return; // Stop pergerakan ngejar kalau player ga ada

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (playerTransform.position.x > transform.position.x && !movingRight)
            {
                movingRight = true;
                Flip();
            }
            else if (playerTransform.position.x < transform.position.x && movingRight)
            {
                movingRight = false;
                Flip();
            }

            if (distanceToPlayer > stoppingDistance)
            {
                float directionX = movingRight ? 1f : -1f;
                rb.linearVelocity = new Vector2(directionX * chaseSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }
        else
        {
            float directionX = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(directionX * speed, rb.linearVelocity.y);
        }
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector3 direction = playerTransform.position - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
        
        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(shootSound);
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
        if (currentHealth <= 0) Die();
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