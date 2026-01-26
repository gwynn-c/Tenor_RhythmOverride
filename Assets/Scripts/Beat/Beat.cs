using UnityEngine;

[System.Serializable]
public class Beat
{
    private string name;
    private BeatHitType type;
    private BeatStatus status;
    
    public Beat(string beatPosition, BeatHitType type)
    {
        name = beatPosition;
        this.type = type;
    }
}