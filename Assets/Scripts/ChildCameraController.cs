using UnityEngine;

public class ChildCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float fixedAngle = 45f; // Isometric angle
    public float height = 10f; // Fixed height above the player
    public float smoothTime = 0.1f; // Smoothing time for camera movement

    [Header("Zoom Settings")]
    public float minZoomDistance = 5f;
    public float maxZoomDistance = 20f;
    public float zoomSpeed = 5f;

    [Header("Pan Settings")]
    public float maxPanDistance = 10f;
    public float panSpeed = 5f;
    public float panSmoothTime = 0.1f; // Smoothing time for panning

    private Vector3 offset;
    private Vector3 panOffset = Vector3.zero;
    private Vector3 panVelocity = Vector3.zero;

    private void Start()
    {
        // Calculate the initial offset based on the fixed isometric angle
        offset = new Vector3(0, height, -height / Mathf.Tan(fixedAngle * Mathf.Deg2Rad));
        transform.localPosition = offset; // Set initial camera position
    }

    private void LateUpdate()
    {
        HandleZoom();
        HandlePan();

        // Apply the pan offset to the camera's local position
        transform.localPosition = offset + panOffset;

        // Ensure the camera looks at the player (with pan offset applied)
        transform.LookAt(transform.parent.position + panOffset);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // Adjust the camera's distance based on scroll input
            float newDistance = offset.magnitude - scrollInput * zoomSpeed;
            newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);

            // Update the offset to maintain the fixed isometric angle
            offset = offset.normalized * newDistance;
        }
    }

    private void HandlePan()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Get mouse position in world space
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                // Calculate the desired pan offset
                Vector3 targetOffset = hit.point - transform.parent.position;
                targetOffset.y = 0; // Keep the pan offset horizontal

                // Clamp the pan offset based on zoom level
                float panLimit = maxPanDistance * (offset.magnitude / maxZoomDistance);
                targetOffset = Vector3.ClampMagnitude(targetOffset, panLimit);

                // Smoothly interpolate the pan offset
                panOffset = Vector3.SmoothDamp(panOffset, targetOffset, ref panVelocity, panSmoothTime);
            }
        }
        else
        {
            // Reset pan offset when Shift is not held
            panOffset = Vector3.SmoothDamp(panOffset, Vector3.zero, ref panVelocity, panSmoothTime);
        }
    }
}