using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

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
    [SerializeField] private string interactText;

    public void Interact(Transform interactor)
    {
        interactorTransform = interactor;
        if (!interactor.GetComponent<PlayerController>().isGunEquipped)
        {
            interactor.GetComponent<PlayerController>().Interacting = true;
            interactor.GetComponent<PlayerUIController>().ShowWeaponInfoPanel(weaponInfo);
        }
        else
        {
            OnInteract?.Invoke();
        }
    }

    public void SelectWeapon()
    {
        interactorTransform.GetComponent<PlayerUIController>().HideWeaponInfoPanel();
        interactorTransform.GetComponent<PlayerController>().SetEquippedGun(gameObject);
        interactorTransform.GetComponent<PlayerController>().Interacting = false;

        Destroy(gameObject.GetComponent<SphereCollider>());
        OnInteract?.Invoke();
        InteractableVFX.SetActive(false);
        Destroy(gameObject.GetComponent<Interactable_Gun>());

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