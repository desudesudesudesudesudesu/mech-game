using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Settings")]
    public float detectionRange = 15f;
    public float attackRange = 10f;
    public float fireDelay = 2f;
    public float rotationSpeed = 2f;
    public LayerMask obstacleMask;

    [Header("References")]
    public Transform rotationPivot;
    public Transform playerTarget;
    //public Transform firePoint;

    private BallisticWeapon weapon;
    private TurretState currentState;
    private float _fireTimer;

    private void Awake()
    {
        weapon = GetComponent<BallisticWeapon>();
        ChangeState(new IdleState(this));
    }

    void Update()
    {
        currentState.Update();

        //DEBUG STUFF----------------------------------------------------------------------------------------
        if (playerTarget != null)
        {
            Debug.DrawLine(transform.position, playerTarget.position,
                HasLineOfSight() ? Color.green : Color.red);
        }

        // Draw forward vector
        Debug.DrawRay(weapon.firePoint.position, weapon.firePoint.forward * 2, Color.cyan);
    }

    public void ChangeState(TurretState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void HandleFiring()
    {//
       // _fireTimer -= Time.deltaTime;
        //if (_fireTimer <= 0)
       // {
            weapon.Fire();
           // _fireTimer = fireDelay;
       // }
    }

    public bool PlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, playerTarget.position) <= detectionRange;
    }

    public bool PlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, playerTarget.position) <= attackRange;
    }

    public bool HasLineOfSight()
    {
        Vector3 directionToPlayer = (playerTarget.position - weapon.transform.position).normalized;
        if (!Physics.Raycast(weapon.transform.position, directionToPlayer,
            Vector3.Distance(weapon.transform.position, playerTarget.position), obstacleMask))
        {
            return true;
        }
        return false;
    }

    public void LookAtPlayer()
    {
        if (playerTarget == null) return;

        Vector3 direction = playerTarget.position - rotationPivot.position;
        direction.y = 0; // Keep rotation horizontal
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rotationPivot.rotation = Quaternion.Slerp(
            rotationPivot.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    public void ResetFireTimer()
    {
       // _fireTimer = fireDelay;
    }
}