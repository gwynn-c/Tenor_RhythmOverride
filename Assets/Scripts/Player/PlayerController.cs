using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    //Components
    Conductor _conductor;
    private MMF_Player _player;
    private StarterAssetsInputs _input;
    //Gun References
    [SerializeField] private GameObject equippedGun;
    public bool isGunEquipped;
    public Transform GunSlot;
    public GameObject GroundSlamVFX;
    [SerializeField] private float slamRadius = 5f;

    public AudioClip[] footstepClips;

    public AudioClip[] runFootStepClips;
    void Start()
    {
        _conductor = Conductor.Instance;
        _input = GetComponent<StarterAssetsInputs>();
        _player = GetComponent<MMF_Player>();
    }

    private void Update()
    {
        isGunEquipped = equippedGun != null;
    }

    // Update is called once per frame
    [SerializeReference] private GameObject spawnedGO;
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

        var cameraShake = _player.GetFeedbackOfType<MMF_CameraShake>();
        cameraShake.CameraShakeProperties.Amplitude = 5;
        cameraShake.CameraShakeProperties.Frequency = 100;
        _player.PlayFeedbacks();
        // SoundFXManager.instance.PlaySingleSoundFXClip();
        if(spawnedGO == null)
        {
            spawnedGO = Instantiate(GroundSlamVFX, slamPosition, Quaternion.identity);
        }
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

    public AudioSource playerAudioSource;
    public void PlayWalkAudio()
    {
        if (playerAudioSource.isPlaying) return;
        playerAudioSource.clip = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length - 1)];
        playerAudioSource.Play();
    }

    public void PlayRunAudio()
    {
        if (playerAudioSource.isPlaying) return;
        playerAudioSource.clip = runFootStepClips[UnityEngine.Random.Range(0, runFootStepClips.Length - 1)];
        playerAudioSource.Play();
    }
    
    public void PlayJumpAudio(AudioClip[] jumpClips, Vector3 position, float volume)
    {
        if (playerAudioSource.isPlaying) return;
        playerAudioSource.clip = jumpClips[UnityEngine.Random.Range(0, jumpClips.Length - 1)];
        playerAudioSource.Play();
    }
}