using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour, IInteractable
{
    
    public void Interact(Transform interactor)
    {
        //Load Next Level
        // SceneManager.LoadScene();
        Debug.Log("Interacted with " + interactor.name);
    }

    public string GetInteractableName()
    {
        return gameObject.name;
    }

    public Transform GetInteractableTransform()
    {
        return transform;
    }
}