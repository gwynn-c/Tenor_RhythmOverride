using System;

public class PlayerEvents
{
    public event Action OnBeatInput;

    public event Action OnBeatMissed;

    public void OnBeatInputPressed()
    {
        OnBeatInput?.Invoke();
    }
    public void OnBeatInputMissed()
    {
        OnBeatMissed?.Invoke();
    }
}