using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Pengaturan Target")]
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    public BoxCollider2D boundsBox;

    private Camera cam;
    private float minX, maxX, minY, maxY;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (boundsBox != null)
        {
            float camHalfHeight = cam.orthographicSize;
            float camHalfWidth = camHalfHeight * cam.aspect;

            minX = boundsBox.bounds.min.x + camHalfWidth;
            maxX = boundsBox.bounds.max.x - camHalfWidth;
            minY = boundsBox.bounds.min.y + camHalfHeight;
            maxY = boundsBox.bounds.max.y - camHalfHeight;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
           
            Vector3 desiredPosition = target.position + offset;

            if (boundsBox != null)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            }

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            transform.position = smoothedPosition;
        }
    }
}