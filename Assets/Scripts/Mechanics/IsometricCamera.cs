using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    public Transform player;          // The player transform to follow
    public float smoothSpeed = 0.125f; // Smoothness of the camera follow
    public Vector3 offset;           // The fixed offset from the player
    public float panSpeed = 5f;      // Speed of panning when holding Shift
    public float maxPanDistance = 5f; // Max distance the camera can pan from the player
    public float panDamping = 0.1f;  // Smooth the pan movement
    public float cursorSensitivity = 0.1f; // Sensitivity for cursor-based panning

    private Vector3 targetPosition;
    private Vector3 panOffset = Vector3.zero;
    private Vector3 currentPanOffset = Vector3.zero;

    void Start()
    {
        // Set the initial target position based on the player and the fixed offset
        if (player != null)
        {
            targetPosition = player.position + offset;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Smooth follow logic
            targetPosition = player.position + offset + panOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Handle panning when Shift key is held down
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                PanCamera();
            }
            else
            {
                // Reset pan offset when Shift key is released
                currentPanOffset = Vector3.Lerp(currentPanOffset, Vector3.zero, panDamping);
                panOffset = currentPanOffset;
            }
        }
    }

    void PanCamera()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // Calculate how far the mouse is from the center of the screen
        Vector3 cursorDirection = (mousePos - screenCenter).normalized;

        // Adjust the camera pan speed based on the mouse's relative position to the screen center
        float panDistanceX = Mathf.Clamp(cursorDirection.x * panSpeed * Time.deltaTime, -maxPanDistance, maxPanDistance);
        float panDistanceY = Mathf.Clamp(cursorDirection.y * panSpeed * Time.deltaTime, -maxPanDistance, maxPanDistance);

        // Apply the calculated pan offset while considering the cursor's position
        Vector3 desiredPanOffset = new Vector3(panDistanceX, panDistanceY, 0);
        currentPanOffset = Vector3.Lerp(currentPanOffset, desiredPanOffset, panDamping * cursorSensitivity);

        // Apply the pan offset, making the camera focus on the cursor
        panOffset = currentPanOffset;
    }

    void LateUpdate()
    {
        // Fixed rotation to ensure the isometric view
        transform.rotation = Quaternion.Euler(30f, 45f, 0f);
    }
}