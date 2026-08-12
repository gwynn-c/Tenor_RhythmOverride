using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private GameObject weaponInfoGameObject;

    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    [SerializeField] private GameObject inputFeedbackContainer;

    public Image DashCooldownFill;
    public Image ThrowableCooldownFill;
    public Image SlamCooldownFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void OnEnable()
    // {
    //     EventManager.instance.playerEvents.OnBeatInput += PlayFeedback;

    // }
    // void OnDisable()
    // {
    //     EventManager.instance.playerEvents.OnBeatInput -= PlayFeedback;

    // }
    // private void PlayFeedback()
    // {
    //     inputFeedbackContainer.SetActive(true);
    // }

    // // Update is called once per frame
    private void Update()
    {
        // if(interactionController.GetInteractableObject() != null) {
        //     Show(interactionController.GetInteractableObject());
        // } else {
        //     Hide();
        // }
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

    public void ShowWeaponInfoPanel(WeaponSO weaponInfo)
    {
        Cursor.lockState = CursorLockMode.Confined;
        weaponInfoGameObject.GetComponent<UIHandler>().InitializePanel(weaponInfo);
        weaponInfoGameObject.SetActive(true);
    }

    public void HideWeaponInfoPanel()
    {
        weaponInfoGameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

}