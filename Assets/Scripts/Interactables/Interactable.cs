using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour, IInteractable
{
    public string interactText;
    public void Interact(Transform interactor)
    {
        //Load Next Level
        SceneManager.LoadScene("Scenes/Playground");
    }

    public string GetInteractableName()
    {
        return interactText;
    }

    public Transform GetInteractableTransform()
    {
        return transform;
    }
}