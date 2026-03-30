using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance {get; private set;}
    [SerializeField] private AudioSource SoundFXSource;
    private void Awake()
    {   
        if(instance != null) 
        {
            Debug.Log("Another instance of SoundFXManager already exists in the scene");
            Destroy(gameObject);
        }
        instance = this;
    }
    public void PlaySoundFXClip(AudioClip[] soundFXClips, Vector3 spawnPosition, float volume)
    {   
        var audioSource = Instantiate(SoundFXSource, spawnPosition, Quaternion.identity);
        audioSource.clip = soundFXClips[Random.Range(0, soundFXClips.Length - 1)];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
    public void PlaySingleSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        var source = Instantiate(SoundFXSource, spawnTransform.position, Quaternion.identity);
        source.PlayOneShot(audioClip, volume);
        float clipRunTime = audioClip.length;
        Destroy(source.gameObject, clipRunTime);
    }
}