public class AttackState : TurretState
{
    public AttackState(EnemyTurret turret) : base(turret) { }

    public override void Enter()
    {
        turret.LookAtPlayer();
    }

    public override void Update()
    {
        if (!turret.PlayerInAttackRange() || !turret.HasLineOfSight())
        {
            turret.ChangeState(new AlertState(turret));
            return;
        }

        turret.LookAtPlayer();
        turret.HandleFiring();
    }
}