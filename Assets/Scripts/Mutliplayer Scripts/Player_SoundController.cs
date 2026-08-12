using UnityEngine;

public class Player_SoundController : MonoBehaviour
{
    public AudioSource playerAudioSource;
    public AudioClip[] footstepClips;

    public AudioClip[] runFootStepClips;
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