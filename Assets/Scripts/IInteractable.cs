using UnityEngine;

public interface IInteractable
{
    void Interact(Transform interactor);
    public string GetInteractableName();
    public Transform GetInteractableTransform();
    
}