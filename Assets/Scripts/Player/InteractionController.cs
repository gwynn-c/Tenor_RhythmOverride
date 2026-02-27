using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactRange = 5f;
    private StarterAssetsInputs _input;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }
    void Update()
    {
        if(_input.interact) 
        {
            IInteractable interactable = GetInteractableObject();
            if(interactable != null) interactable.Interact(transform);
        }
    }

    public IInteractable GetInteractableObject(){
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach(Collider collider in colliderArray){
            if(collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }
        IInteractable closestInteractable = null;
        foreach(IInteractable interactable in interactableList){
            if(closestInteractable == null){
                closestInteractable = interactable;
            } else {
                if(Vector3.Distance(transform.position, interactable.GetInteractableTransform().transform.position) < Vector3.Distance(transform.position, closestInteractable.GetInteractableTransform().transform.position))
                    closestInteractable = interactable;
            }
        }
        return closestInteractable;
    }
}