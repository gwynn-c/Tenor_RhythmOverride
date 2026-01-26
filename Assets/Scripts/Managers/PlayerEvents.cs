using System;

public class PlayerEvents
{
    public event Action OnBeatInput;


    public void OnBeatInputPressed()
    {
        OnBeatInput?.Invoke();
    }
}