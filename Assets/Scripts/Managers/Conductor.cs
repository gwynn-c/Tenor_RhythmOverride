using System.Collections;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class Conductor : MonoBehaviour
{
    public static Conductor Instance {get; private set;}
    public AudioSource audioSource;
    public SongData[] songs;
    private int songIndex;
    public AudioSource secondaryAudioSource;
    [Header("Song Settings")]
    [Space(10)]

    public float beatsPerMinute;
    public float secondsPerBeat;
    public float songPositionInBeats;
    public float dspSongTime;
    public float songPosition;

    // [Header("Loop Settings")]
    // [Space(10)]
    // //If Looping Audio
    // public float firstBeatOffset;
    // public float beatsPerLoop;
    // public int completedLoops;
    // public float loopPositionInBeats;
    public AudioClip positveFeedback;
    public AudioClip negativeFeedback;
    [Header("Streak Settings")]
    [Space(10)]
    public int currentStreak;
    public TextMeshProUGUI streakText;
    private void OnEnable()
    {
        EventManager.instance.playerEvents.OnBeatInput += IncrementScore;
        EventManager.instance.playerEvents.OnBeatMissed += ResetStreak;
    }

    private void OnDisable()
    {
        EventManager.instance.playerEvents.OnBeatInput -= IncrementScore;
        EventManager.instance.playerEvents.OnBeatMissed -= ResetStreak;
        
    }
    
    public void Awake()
    {
        songIndex = 0;
        if(Instance == null)
            Instance = this;
        beatsPerMinute = songs[songIndex].beatsPerMinute;
        audioSource.clip = songs[songIndex].song;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        secondsPerBeat = 60f/beatsPerMinute;
        dspSongTime = (float)AudioSettings.dspTime;
        
        
        var crosshair = GetComponent<MMF_Player>();
        crosshair.GetFeedbackOfType<MMF_Scale>().AnimateScaleDuration = secondsPerBeat;
        crosshair.GetFeedbackOfType<MMF_Scale>().Timing.CooldownDuration = secondsPerBeat;
        crosshair.GetFeedbackOfType<MMF_Scale>().SetDelayBetweenRepeats(secondsPerBeat);

        crosshair.GetFeedbackOfType<MMF_ImageAlpha>().Duration = secondsPerBeat;
        crosshair.GetFeedbackOfType<MMF_ImageAlpha>().SetDelayBetweenRepeats(secondsPerBeat);

    }

    void IncrementScore()
    {
        currentStreak++;
        PlayFeedback(positveFeedback);
    }

    void ResetStreak()
    {
        if(currentStreak >= 5)
        {
            PlayFeedback(negativeFeedback);
        }
        currentStreak = 0;
    }

    private void PlayFeedback(AudioClip audioClip)
    {
        if (secondaryAudioSource.isPlaying) return;
        secondaryAudioSource.clip = audioClip;
        secondaryAudioSource.Play();
    }

    void Update()
    {
        if(!audioSource.isPlaying) return;
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        
        songPositionInBeats = songPosition / secondsPerBeat;
        streakText.text = "Streak: " + currentStreak.ToString();
    }

    public float GetSongPosition()
    {
        return songPosition;
    }

    public float GetCompleteSongPosition()
    {
        return audioSource.clip.length;
    }

    public void NextSong()
    {
        if (songIndex >= songs.Length - 1)
            songIndex = 0 ;  
        else
            songIndex++;

        audioSource.clip = songs[songIndex].song;   
        beatsPerMinute = songs[songIndex].beatsPerMinute;
        PlaySong();

    }
    public void PreviousSong()
    {
        if (songIndex <= 0)
            songIndex = songIndex - 1;
        else 
            songIndex--;
        audioSource.clip = songs[songIndex].song;   
        beatsPerMinute = songs[songIndex].beatsPerMinute;
        
        PlaySong();
    }

    public void PauseSong()
    {
        audioSource.Pause();
    }

    public void PlaySong()
    {
        audioSource.Play();
        songPositionInBeats = 0;
        songPosition = 0;
    }
    public void StopSong()
    {
        audioSource.Pause();
        audioSource.time = 0;
    }
}