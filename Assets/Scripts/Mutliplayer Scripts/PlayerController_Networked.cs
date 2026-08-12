using Unity.Netcode;
using UnityEngine;
using MoreMountains.Feedbacks;
using StarterAssets;
using UnityEngine.InputSystem;
public class PlayerController_Networked : NetworkBehaviour
{
    private MMF_Player _player;

    [SerializeField] private GameObject equippedGun;
    public bool isGunEquipped;
    public Transform GunSlot;
    public void SetEquippedGun(GameObject gunPrefab)
    {
        if (!isGunEquipped) equippedGun = gunPrefab;
        else
        {
            //Pop up for confirmation
        }
        // equippedGun.GetComponent<GunController>().Initialize();
        equippedGun.transform.SetParent(GunSlot);
        equippedGun.transform.rotation = GunSlot.rotation;
        equippedGun.transform.position = GunSlot.position;
        equippedGun.transform.localScale = Vector3.one;
    }
}


public class InputHandler : MonoBehaviour
{
    private InputActionAsset inputActions;

    private InputAction _mInteract;
    private InputAction _mShoot;
    private InputAction _mDash;
    private InputAction _mLook;
    private InputAction _mMove;


    private InteractionController interactionController;


    void Update()
    {
        _mInteract.performed += context =>
        {

        };
        GetComponent<FirstPersonController_Networked>().Move(GetComponent<CharacterController>(), _mMove.ReadValue<Vector2>());
    }


}
