public class IdleState : TurretState
{
    public IdleState(EnemyTurret turret) : base(turret) { }

    public override void Enter()
    {
        turret.ResetFireTimer();
    }

    public override void Update()
    {
        if (turret.PlayerInDetectionRange())
        {
            turret.ChangeState(new AlertState(turret));
        }
    }
}