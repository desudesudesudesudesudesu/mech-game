using UnityEngine;
using static UnityEngine.Rendering.STP;

public class MechMovement : MonoBehaviour
{

    public MechData mechData;   

    [Header("References")]
    public Transform legs;
    public Transform torso;
    public Transform turret;
    public LayerMask targetLayer;

    private Rigidbody rb;
    public Camera mainCamera;
    private float currentSpeed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        // Null checks for critical references
        if (legs == null || torso == null || turret == null)
        {
            Debug.LogError("Critical transforms (legs, torso, turret) are not assigned in the Inspector!");
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleAiming();

        // Debug
        Debug.DrawRay(turret.position, turret.forward * 10f, Color.yellow);
    }

    private void HandleMovement()
    {
        // WASD tank-like controls
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float targetSpeed = vertical * mechData.moveSpeed;

        if (Mathf.Abs(vertical) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, mechData.acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, mechData.deceleration * Time.deltaTime);
        }

        // Rotate legs
        legs.Rotate(Vector3.up * horizontal * mechData.legTurnSpeed * Time.deltaTime);

        // Move forward
        rb.MovePosition(rb.position + legs.forward * currentSpeed * Time.deltaTime);
    }

    private void HandleAiming()
    {
        // Get mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, targetLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = torso.position.y;

            // Rotate torso horizontally towards mouse
            Vector3 torsoDirection = (targetPos - torso.position).normalized;
            Quaternion torsoRotation = Quaternion.LookRotation(torsoDirection);
            torso.rotation = Quaternion.Slerp(torso.rotation, torsoRotation, mechData.torsoRotationSpeed * Time.deltaTime);

            // Rotate turret vertically
            Vector3 turretDirection = hit.point - turret.position;
            Vector3 horizontalDirection = Vector3.ProjectOnPlane(turretDirection, Vector3.up).normalized;

            // Calculate the desired pitch angle
            float pitchAngle = Vector3.SignedAngle(horizontalDirection, turretDirection.normalized, turret.right);
            //pitchAngle = Mathf.Clamp(pitchAngle, mechData.minTurretPitch, mechData.maxTurretPitch);
            pitchAngle = Mathf.Clamp(pitchAngle, mechData.minTurretPitch, mechData.maxTurretPitch);
            turret.localRotation = Quaternion.Euler(
                Mathf.MoveTowards(turret.localEulerAngles.x, pitchAngle, mechData.turretPitchSpeed * Time.deltaTime),
                0,
                0
            );

            // Smoothly interpolate the turret's vertical rotation
            Quaternion targetTurretRotation = Quaternion.Euler(pitchAngle, turret.localEulerAngles.y, turret.localEulerAngles.z);
            turret.localRotation = Quaternion.Slerp(turret.localRotation, targetTurretRotation, mechData.torsoRotationSpeed * Time.deltaTime); //sharing torso speed for now

            Debug.DrawLine(turret.position, hit.point, Color.red);
        }
    }
}