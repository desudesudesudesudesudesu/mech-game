using UnityEngine;

public class MechMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float legRotationSpeed = 90f;
    public float acceleration = 2f;
    public float deceleration = 4f;

    [Header("Aiming Settings")]
    public float torsoRotationSpeed = 2f;
    public float turretMaxPitch = 30f;
    public float turretMinPitch = -10f;

    [Header("References")]
    public Transform legs;
    public Transform torso;
    public Transform turret;
    public LayerMask groundLayer;

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

        float targetSpeed = vertical * maxSpeed;

        if (Mathf.Abs(vertical) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        // Rotate legs
        legs.Rotate(Vector3.up * horizontal * legRotationSpeed * Time.deltaTime);

        // Move forward
        rb.MovePosition(rb.position + legs.forward * currentSpeed * Time.deltaTime);
    }

    private void HandleAiming()
    {
        // Get mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = torso.position.y;

            // Rotate torso horizontally towards mouse
            Vector3 torsoDirection = (targetPos - torso.position).normalized;
            Quaternion torsoRotation = Quaternion.LookRotation(torsoDirection);
            torso.rotation = Quaternion.Slerp(torso.rotation, torsoRotation, torsoRotationSpeed * Time.deltaTime);

            // Rotate turret vertically
            Vector3 turretDirection = hit.point - turret.position;
            Vector3 horizontalDirection = Vector3.ProjectOnPlane(turretDirection, Vector3.up).normalized;

            // Calculate the desired pitch angle
            float pitchAngle = Vector3.SignedAngle(horizontalDirection, turretDirection.normalized, turret.right);
            pitchAngle = Mathf.Clamp(pitchAngle, turretMinPitch, turretMaxPitch);

            // Smoothly interpolate the turret's vertical rotation
            Quaternion targetTurretRotation = Quaternion.Euler(pitchAngle, turret.localEulerAngles.y, turret.localEulerAngles.z);
            turret.localRotation = Quaternion.Slerp(turret.localRotation, targetTurretRotation, torsoRotationSpeed * Time.deltaTime); //sharing torso speed for now

            Debug.DrawLine(turret.position, hit.point, Color.red);
        }
    }
}