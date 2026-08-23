using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class PlayerVergant : MonoBehaviour
{
    public Image healthImage; 

    [Header("Sprite")]
    public Sprite[] healthSprites; 

    [Header("Damage")]
    public AudioSource audioSource;
    public AudioClip[] damageSounds; 

    [Header("Footsteps & Jump")]
    public AudioClip[] footstepSounds;
    public AudioClip jumpSound;
    public float stepInterval = 0.4f;
    private float stepTimer;

    [Header("Status Nyawa")]
    public int maxHealth = 5;
    public int currentHealth;

    [HideInInspector]
    public bool isGrounded = true;

    private Vector3 originalUIPosition;
    private Quaternion originalUIRotation;

    void Start()
    {
        currentHealth = maxHealth;
        
        if (healthImage != null)
        {
            healthImage.rectTransform.pivot = new Vector2(0f, 1f);
            originalUIPosition = healthImage.rectTransform.anchoredPosition;
            originalUIRotation = healthImage.rectTransform.localRotation;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        UpdateHealthUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(1);
        }

        HandleFootsteps();
    }

    public void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void HandleFootsteps()
    {
        if (!isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; 
        }
    }

    void PlayFootstepSound()
    {
        if (audioSource != null && footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);

            if (footstepSounds[randomIndex] != null)
            {
                audioSource.PlayOneShot(footstepSounds[randomIndex]);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthUI();
        PlayDamageSound();

        if (healthImage != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(DamageEffectRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void PlayDamageSound()
    {
        if (audioSource != null && damageSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, damageSounds.Length);
            
            if (damageSounds[randomIndex] != null)
            {
                audioSource.PlayOneShot(damageSounds[randomIndex]);
            }
        }
    }

    void UpdateHealthUI()
    {
        if (currentHealth > 0 && currentHealth <= healthSprites.Length)
        {
            healthImage.gameObject.SetActive(true);
            healthImage.sprite = healthSprites[currentHealth - 1];
        }
        else if (currentHealth <= 0)
        {
            healthImage.gameObject.SetActive(false); 
        }
    }

    IEnumerator DamageEffectRoutine()
    {
        float duration = 0.25f; 
        float magnitude = 2f;   
        float rotationMagnitude = 5f; 
        Color originalColor = healthImage.color;

        float elapsed = 0.0f;

        healthImage.color = Color.gray;

        while (elapsed < duration)
        {
            float x = originalUIPosition.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalUIPosition.y + Random.Range(-1f, 1f) * magnitude;
            healthImage.rectTransform.anchoredPosition = new Vector2(x, y);

            float rotZ = Random.Range(-1f, 1f) * rotationMagnitude;
            healthImage.rectTransform.localRotation = Quaternion.Euler(0, 0, rotZ);

            elapsed += Time.deltaTime;
            yield return null; 
        }

        healthImage.rectTransform.anchoredPosition = originalUIPosition;
        healthImage.rectTransform.localRotation = originalUIRotation;
        healthImage.color = originalColor;
    }

    void Die()
    {
        Debug.Log("Player Mati!");
    }
}