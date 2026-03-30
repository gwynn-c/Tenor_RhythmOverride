using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Interactable_GroundSlam : MonoBehaviour, IGroundSlamTargetable, IInteractable
{
    public UnityEvent onInteract;
    [SerializeField] private string interactText;
    [SerializeField] private float stunDuration = 1.2f;


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

    public void Interact(Transform interactor)
    {
        onInteract?.Invoke();

    }

    public string GetInteractableName()
    {
        return interactText;
    }

    public Transform GetInteractableTransform()
    {
        return transform;
    }

    public void Stun()
    {
        StartCoroutine(nameof(StunDuration), .1f);
    }

    private IEnumerator StunDuration()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        var temp = stunDuration;
        while (temp > 0)
        {
            temp -= Time.deltaTime;
            yield return null;
        }
        
        GetComponent<NavMeshAgent>().isStopped = false;
        temp = stunDuration;
    }
}