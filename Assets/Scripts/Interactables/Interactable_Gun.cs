using UnityEngine;
using UnityEngine.Events;

public class Interactable_Gun : MonoBehaviour, IInteractable
{
    public GameObject InteractableVFX;
    public UnityEvent OnInteract;

    public WeaponSO weaponInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Transform interactorTransform;
    public void Interact(Transform interactor)
    {
        interactorTransform = interactor;
        if (!interactor.GetComponent<PlayerController>().isGunEquipped)
        {
            interactor.GetComponent<PlayerUIController>().ShowWeaponInfoPanel(weaponInfo);
        }

    }

    public void SelectWeapon()
    {
        interactorTransform.GetComponent<PlayerUIController>().HideWeaponInfoPanel();
        interactorTransform.GetComponent<PlayerController>().SetEquippedGun(gameObject);
        Destroy(gameObject.GetComponent<SphereCollider>());
        OnInteract?.Invoke();
        InteractableVFX.SetActive(false);
        Destroy(gameObject.GetComponent<Interactable_Gun>());

    }

    public string GetInteractableName()
    {
        return name;
    }

    public Transform GetInteractableTransform()
    {
        return transform;
    }
}