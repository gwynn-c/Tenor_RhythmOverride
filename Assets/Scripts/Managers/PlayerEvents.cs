using System;

public class PlayerEvents
{
    public event Action OnBeatInput;

    public event Action OnBeatMissed;
    public event Action EnemyKilled;

    public void OnBeatInputPressed()
    {
        OnBeatInput?.Invoke();
    }
    public void OnBeatInputMissed()
    {
        OnBeatMissed?.Invoke();
    }

    public void OnEnemyKilled()
    {
        EnemyKilled?.Invoke();
    }
}