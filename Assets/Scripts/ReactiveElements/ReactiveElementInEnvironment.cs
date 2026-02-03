using System.Collections;
using UnityEngine;
using MoreMountains.Feedbacks;
public class ReactiveElementInEnvironment : MonoBehaviour, IReactive
{
    private MMF_Player player;
    public float reactToStep;
    public IEnumerator Start()
    {
        player = GetComponent<MMF_Player>();
        yield return new WaitUntil(() => Conductor.Instance != null);
        ReactToTheBeat(player);
    }
    
    public void ReactToTheBeat(MMF_Player player)
    {
        player.GetFeedbackOfType<MMF_Scale>().AnimateScaleDuration = Conductor.Instance.secondsPerBeat/reactToStep;
        player.GetFeedbackOfType<MMF_Scale>().SetDelayBetweenRepeats(Conductor.Instance.secondsPerBeat);
        player.InitialDelay = Conductor.Instance.secondsPerBeat;
        player.PlayFeedbacks();
    }

    public void ChangeBeatsPerMinute()
    {
        throw new System.NotImplementedException();
    }
}