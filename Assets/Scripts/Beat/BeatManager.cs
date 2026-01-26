using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour
{
   public List<Beat> BeatsInLoop = new List<Beat>();

   /// <summary>
   /// First Initialize all the Beats in the first pass of the song loop
   /// Get the total number of beats in a song Loop => Length of Song in Seconds / SecondsPerBeats
   /// </summary>
   public int totalBeats;
   private IEnumerator Start()
   {
      yield return new WaitForSeconds(5f);
      totalBeats = Mathf.FloorToInt(Conductor.Instance.GetCompleteSongPosition() - 1 / Conductor.Instance.secondsPerBeat);
      InitializeBeats();
   }
   private void InitializeBeats()
   {
      BeatsInLoop.Clear();
      for (var i = 0; i < totalBeats; i++)
      {
         BeatsInLoop.Add(new Beat("Beat Position: " + i,BeatHitType.Disabled));
      }
   }
}