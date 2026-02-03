using UnityEngine;
using UnityEngine.Events;

public class Interactable_GroundSlam : MonoBehaviour, IGroundSlamTargetable
{
    public UnityEvent onInteract;
    

    public Transform GetTransform()
    {
        return transform;
    }

    public void DirectSlam()
    {
        //Invoke?OpenDoor
        onInteract?.Invoke();
    }
    

    // ReSharper disable Unity.PerformanceAnalysis
    public void WithinSlamRadius(float distanceFromEpicenter)
    {
        Debug.Log("Is within: " + distanceFromEpicenter + " of the player|epicenter");
    }
}