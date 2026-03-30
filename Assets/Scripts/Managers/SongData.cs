using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongData : ScriptableObject
{
    public AudioClip song;
    public int beatsPerMinute;
    public string songName;
    

}