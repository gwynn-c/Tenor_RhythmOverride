using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    public PlayerEvents playerEvents;


    private void Awake()
    {
        instance = this;
        playerEvents = new PlayerEvents();
    }

}