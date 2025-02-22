using UnityEngine;

[CreateAssetMenu(fileName = "MechData", menuName = "Data/Mech")]
public class MechData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float reverseSpeed = 3f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float legTurnSpeed = 10f;
    public float rotationSpeed = 10f;

    [Header("Aiming")]
    //public float torsoTurnSpeed = 90f; // use rotation speed
    public float turretPitchSpeed = 45f; // Vertical rotation (deg/sec)
    public float maxTurretPitch = 30f; // Clamp vertical angle
    public float minTurretPitch = -40f;
    public float torsoRotationSpeed = 90f;
}