using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class FeedbackUIHandler : MonoBehaviour
{
    private TextMeshProUGUI feedbackText;
    private Conductor conductor;
    private MMF_Player player;
    
    public float feedbackSpeed = .6f;
    [SerializeField] private float feedbackDuration;

    public Color[] feedbackColors = new[]
    {
        new Color(209,258,255,255),
        Color.green,
        Color.yellow,
        Color.orange,
        Color.red
    };

    public string[] feedbackPhrases = new[]
    {
        "Nice",
        "Awesome",
        "Fantastic",
        "You are on FIRE!"
    };
    
    public float screenBoundsX, screenBoundsY;
    public float maxScreenWidth;
    public float maxScreenHeight;
    [SerializeField] private float offset = 10f;


    public void Awake()
    {
        feedbackText = GetComponent<TextMeshProUGUI>();
        conductor = Conductor.Instance;
        player = GetComponent<MMF_Player>();
        
        maxScreenWidth = Screen.width - screenBoundsX;
        maxScreenHeight = Screen.height - screenBoundsY;
    }

    private void OnEnable()
    {
        var position = new Vector3(Random.Range(0, maxScreenWidth / 2), Random.Range(0, maxScreenHeight / 2), 0);
        var temp = player.GetFeedbackOfType<MMF_Position>();
            
        temp.InitialPosition = position;
        temp.DestinationPosition = position + new Vector3(position.x, position.y + offset, 0);
        temp.AnimatePositionDuration = feedbackDuration;
        player.Initialization();
        player.PlayFeedbacks();
        var streak = conductor.currentStreak;
        feedbackText.color = streak switch
        {
            <= 5 => feedbackColors[0],
            <= 15 => feedbackColors[1],
            <= 25 => feedbackColors[2],
            <= 30 => feedbackColors[3],
            _ => feedbackText.color
        };

        feedbackText.text = feedbackPhrases[Random.Range(0, feedbackPhrases.Length)];
        Invoke(nameof(Reset), feedbackDuration);
    }

    private void Reset()
    {
        gameObject.SetActive(false);
    }
}