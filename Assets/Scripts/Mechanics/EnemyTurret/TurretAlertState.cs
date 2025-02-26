public class AlertState : TurretState
{
    public AlertState(EnemyTurret turret) : base(turret) { }

    public override void Update()
    {
        if (turret.PlayerInAttackRange() && turret.HasLineOfSight())
        {
            turret.ChangeState(new AttackState(turret));
        }
        else if (!turret.PlayerInDetectionRange())
        {
            turret.ChangeState(new IdleState(turret));
        }
    }
}