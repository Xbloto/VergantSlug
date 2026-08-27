using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 15f; 
    public float lifeTime = 5f;
    public int damageAmount = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ProsesHantaman(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ProsesHantaman(collision.gameObject);
    }

    void ProsesHantaman(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            PlayerVergant player = obj.GetComponent<PlayerVergant>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }
}