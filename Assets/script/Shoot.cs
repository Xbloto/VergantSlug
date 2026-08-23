using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float fireRate = 0.2f;   

    public AudioSource audioSource;
    public AudioClip shootSound;
    public float minPitch = 0.85f;
    public float maxPitch = 1.15f;

    private bool isShooting = false;
    private float nextFireTime = 0f;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            isShooting = !isShooting; 

            if (!isShooting)
            {
                transform.rotation = Quaternion.identity; 
            }
        }

        if (isShooting)
        {
            AimAtMouseAndRotate(); 

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void AimAtMouseAndRotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;

        Vector3 currentScale = transform.localScale;
        if (direction.x < 0) 
        {
            currentScale.x = -Mathf.Abs(currentScale.x); 
        } 
        else 
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }
        transform.localScale = currentScale;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (currentScale.x < 0)
        {
            angle += 180f;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(shootSound);
        }
    }
}