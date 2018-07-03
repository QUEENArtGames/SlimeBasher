using UnityEngine;

public class SlimeAudio : MonoBehaviour
{

    private AudioSource _movementSource;
    public AudioClip DeathClip;
    public AudioClip SlimeStepClip;

    // Use this for initialization
    void Awake()
    {
        _movementSource = GetComponent<AudioSource>();
    }

    public void PlayMovementClip()
    {
        _movementSource.clip = SlimeStepClip;
        _movementSource.Play();
    }

    public void PlayDeathClip()
    {
        if (_movementSource.isPlaying)
            return;

        _movementSource.clip = DeathClip;
        _movementSource.Play();
    }

}
