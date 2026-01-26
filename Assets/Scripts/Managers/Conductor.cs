using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class Conductor : MonoBehaviour
{
    public static Conductor Instance {get; private set;}
    public AudioSource audioSource;
    
    [Header("Song Settings")]
    public float beatsPerMinute;
    public float secondsPerBeat;
    public float songPositionInBeats;
    public float dspSongTime;
    public float songPosition;
    
    [Header("Loop Settings")]
    [Space(10)]
    //If Looping Audio
    public float firstBeatOffset;
    public float beatsPerLoop;
    public int completedLoops;
    public float loopPositionInBeats;

    [Header("Streak Settings")]
    [Space(10)]
    public int currentStreak;
    
    private void OnEnable()
    {
        EventManager.instance.playerEvents.OnBeatInput += IncrementScore;
    }

    private void OnDisable()
    {
        EventManager.instance.playerEvents.OnBeatInput -= IncrementScore;
    }
    
    public void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secondsPerBeat = 60f/beatsPerMinute;
        dspSongTime = (float)AudioSettings.dspTime;
        
        audioSource.Play();
    }

    void IncrementScore()
    {
        currentStreak++;
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        
        songPositionInBeats = songPosition / secondsPerBeat;
    }
    
    public float GetSongPosition()
    {
        return songPosition;
    }

    public float GetCompleteSongPosition()
    {
        return audioSource.clip.length;
    }
}
