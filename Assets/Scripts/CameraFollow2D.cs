using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Camera Target")]
    public Transform target;

    [Header("Arena Boundary")]
    public BoxCollider2D arenaBounds;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;

    [Header("Player Position")]
    // Positive value moves the camera above the player,
    // making the player appear lower on the screen
    public float verticalOffset = 1f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || arenaBounds == null)
            return;

        // Follow the player with a vertical offset
        Vector3 desiredPosition = new Vector3(
            target.position.x,
            target.position.y + verticalOffset,
            transform.position.z
        );

        // Smooth camera movement
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        Bounds bounds = arenaBounds.bounds;

        // Calculate the camera's visible size
        float cameraHalfHeight = cam.orthographicSize;
        float cameraHalfWidth =
            cameraHalfHeight * cam.aspect;

        // Keep the camera inside the arena
        float minX = bounds.min.x + cameraHalfWidth;
        float maxX = bounds.max.x - cameraHalfWidth;

        float minY = bounds.min.y + cameraHalfHeight;
        float maxY = bounds.max.y - cameraHalfHeight;

        float clampedX =
            minX <= maxX
                ? Mathf.Clamp(smoothedPosition.x, minX, maxX)
                : bounds.center.x;

        float clampedY =
            minY <= maxY
                ? Mathf.Clamp(smoothedPosition.y, minY, maxY)
                : bounds.center.y;

        transform.position = new Vector3(
            clampedX,
            clampedY,
            transform.position.z
        );
    }
}