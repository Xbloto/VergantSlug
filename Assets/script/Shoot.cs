using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Senjata")]
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float fireRate = 0.2f;   

    private bool isShooting = false;
    private float nextFireTime = 0f;

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
    }
}