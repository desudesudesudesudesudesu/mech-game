public abstract class TurretState
{
    protected EnemyTurret turret;

    public TurretState(EnemyTurret turret)
    {
        this.turret = turret;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
