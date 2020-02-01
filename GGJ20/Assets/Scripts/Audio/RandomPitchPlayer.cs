using UnityEngine;

public class RandomPitchPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySFX(AudioClip audioClip,float minPitch,float maxPitch)
    {
        audioSource.pitch = (Random.Range(minPitch, maxPitch));
        audioSource.PlayOneShot(audioClip);
    }
}
