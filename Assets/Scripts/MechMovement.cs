using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MechMovement : MonoBehaviour
{


    [Header("Movement settings")]
    public float maxSpeed = 10f;
    public float legRotationSpeed = 90f;
    public float acceleration = 2f;
    public float deceleration = 4f;

    [Header("Aiming Settings")]
    public float torsoRotationSpeed = 2f;
    public float turretMaxPitch = 30f;
    public float turrenMinPitch = -10f;

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
        //mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAiming();

        //Debug
        Debug.DrawRay(turret.position, turret.forward * 10f, Color.yellow);
    }

    void HandleMovement() {
        //WASD tank-like controls
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float targetSpeed = vertical * maxSpeed;

        if (Mathf.Abs(vertical) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        //decelerate to zero when no input
        else {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        //rotate legs
        legs.Rotate(Vector3.up * horizontal * legRotationSpeed * Time.deltaTime);
        //move foward        
        rb.linearVelocity = legs.forward * currentSpeed;       
        
    }

    void HandleAiming() {

        //get mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = torso.position.y;

            //rotate torso horizontally towards mouse
            Vector3 torsoDirection = (targetPos - torso.position).normalized;
            Quaternion torsoRotation = Quaternion.LookRotation(torsoDirection);
            torso.rotation = Quaternion.Slerp(torso.rotation, torsoRotation, torsoRotationSpeed * Time.deltaTime);

            //rotate turrets vertically 
            Vector3 turretDirection = hit.point - turret.position;

            //project direction onto the vertical  plane relative to the turrets forward
            Vector3 horizontalDirection = Vector3.ProjectOnPlane(turretDirection, Vector3.up).normalized;

            //float pitchAngle = Mathf.Clamp(-Vector3.SignedAngle(turretDirection, Vector3.up, turret.right), turrenMinPitch, turretMaxPitch);
            float pitchAngle = Vector3.SignedAngle(horizontalDirection, turretDirection.normalized, turret.right);
            pitchAngle = Mathf.Clamp(pitchAngle, turrenMinPitch, turretMaxPitch);
            turret.localEulerAngles = new Vector3(pitchAngle, 0, 0);

            Debug.DrawLine(turret.position, hit.point, Color.red);            
        }
    }
}
