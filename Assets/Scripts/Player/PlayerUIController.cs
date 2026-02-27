using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private GameObject containerGameObject;

    [SerializeField] TextMeshProUGUI interactTextMeshProUGUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update(){
        if(interactionController.GetInteractableObject() != null) {
            Show(interactionController.GetInteractableObject());
        } else {
            Hide();
        }
    }
    
    private void Show(IInteractable interactable){
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = "E to pick up " + interactable.GetInteractableName();
    }   
    private void Hide(){
        containerGameObject.SetActive(false);
    }
}
