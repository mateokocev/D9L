using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public Transform targetPlayer;    // Reference to the player's transform
    public float smoothCameraSpeed = 0.2f;    // Smoothness of the camera movement

    // Reference to the boundary trigger collider
    public Collider2D boundaryCollider;

    private Vector3 cameraSize;

    void Start()
    {
        if (boundaryCollider != null)
        {
            // Get the size of the boundary collider
            cameraSize = new Vector3(boundaryCollider.bounds.size.x, boundaryCollider.bounds.size.y, 0);
        }
    }

    void LateUpdate()
    {
        if (targetPlayer != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = targetPlayer.position + new Vector3(0, 0, -10);

            // Smoothly move the camera to the desired position
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothCameraSpeed);

            if (boundaryCollider != null)
            {
                // Apply boundaries to the camera position if a boundary is defined
                Vector3 clampedPosition = ClampPositionToBoundary(smoothPosition);
                transform.position = clampedPosition;
            }
            else
            {
                // Just follow the player without boundaries
                transform.position = smoothPosition;
            }
        }
    }

    private Vector3 ClampPositionToBoundary(Vector3 position)
    {
        // Clamp the camera's position within the boundary collider's bounds
        Vector3 boundaryMin = boundaryCollider.bounds.min + cameraSize / 2;
        Vector3 boundaryMax = boundaryCollider.bounds.max - cameraSize / 2;

        float clampedX = Mathf.Clamp(position.x, boundaryMin.x, boundaryMax.x);
        float clampedY = Mathf.Clamp(position.y, boundaryMin.y, boundaryMax.y);

        return new Vector3(clampedX, clampedY, position.z);
    }
}
