using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ReactiveGroupedElements : MonoBehaviour, IReactive
{
    [SerializeField] private MMF_Player[] players;
    public float reactToStep;
    public IEnumerator Start()
    {
        yield return new WaitUntil(() => Conductor.Instance != null);

        foreach (var p in players)
        {
            ReactToTheBeat(p);
        }
    }
    
    public void ReactToTheBeat(MMF_Player player)
    {
        player.InitialDelay = Conductor.Instance.secondsPerBeat;
        
        player.GetFeedbackOfType<MMF_Scale>().AnimateScaleDuration = Conductor.Instance.secondsPerBeat/Random.Range(1, reactToStep);
        player.GetFeedbackOfType<MMF_Scale>().SetDelayBetweenRepeats(Conductor.Instance.secondsPerBeat);
        
        player.PlayFeedbacks();
    }

    public void ChangeBeatsPerMinute()
    {
        throw new System.NotImplementedException();
    }
}
