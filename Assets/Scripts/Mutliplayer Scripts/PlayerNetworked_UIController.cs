using UnityEngine;
using TMPro;
public class PlayerNetworked_UIController : MonoBehaviour
{
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private GameObject containerGameObject;

    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;



    private void Start()
    {
        interactionController = GetComponent<InteractionController>();
    }
    private void Update()
    {
        if (interactionController.GetInteractableObject() != null)
        {
            Show(interactionController.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(IInteractable interactable)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractableName();
    }
    private void Hide()
    {
        containerGameObject.SetActive(false);
    }
}