using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageInterval = 1f;

    private float damageTimer;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                PlayerVergant playerHealth = collision.gameObject.GetComponent<PlayerVergant>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }

                damageTimer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageTimer = 0f;
        }
    }
}