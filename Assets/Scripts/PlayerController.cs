using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    //Components
    Conductor _conductor;

    private StarterAssetsInputs _input;
    //Gun References
    [SerializeField] private GameObject equippedGun;
    public bool isGunEquipped;
    public Transform GunSlot;
        

    public GameObject GroundSlamVFX;
    [SerializeField] private float slamRadius = 5f;
    void Start()
    {
        _conductor = Conductor.Instance;
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        isGunEquipped = equippedGun != null;
    }

    // Update is called once per frame
    
    public void GroundSlam(float offset)
    {
        //The Transform position of where the player's feet landed in relation to the player controller offset
        var slamPosition = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
        //Colliders of all within the overlap sphere of the ground slam
        var checkSphereSlammable = Physics.OverlapSphere(slamPosition, slamRadius);
        //Check if an entity is within direct radius or further from the slam
        foreach (var slammedEntity in checkSphereSlammable)
        {
            if (slammedEntity.TryGetComponent<Interactable_GroundSlam>(out var slammed))
            {
                    slammed.DirectSlam();
            }
            
        }
        // Instantiate(GroundSlamVFX, slamPosition, Quaternion.identity);
    }


    public void SetEquippedGun(GameObject gunPrefab)
    {
        if (!isGunEquipped) equippedGun = gunPrefab;
        else
        {
            //Pop up for confirmation
        }
        equippedGun.GetComponent<GunController>().Initialize(_input);
        equippedGun.transform.SetParent(GunSlot);
        equippedGun.transform.rotation = GunSlot.rotation;
        equippedGun.transform.position = GunSlot.position;
        equippedGun.transform.localScale = Vector3.one;
    }
    
}