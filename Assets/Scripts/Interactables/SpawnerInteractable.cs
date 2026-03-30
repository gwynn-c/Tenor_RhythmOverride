using UnityEngine;
using UnityEngine.Events;

public class SpawnerInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;
    public string interactText;

    public void Interact(Transform interactor)
    {
            OnInteract.Invoke();
    }
    
    public string GetInteractableName()
    {
        return interactText;
    }
    
    public Transform GetInteractableTransform()
    {
        return transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnInteract.Invoke();
        }
    }
    
}