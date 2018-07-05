using UnityEngine;

public class SlimeAudio : MonoBehaviour
{

    public AudioSource MovementSource;
    public AudioSource DamageSource;
    public AudioClip DeathClip;
    public AudioClip SlimeStepClip;

    // Use this for initialization
    void Awake()
    {
        MovementSource = GetComponent<AudioSource>();
    }

    public void PlayMovementClip()
    {
        MovementSource.clip = SlimeStepClip;
        MovementSource.Play();
    }

    public void PlayDeathClip()
    {
        if (MovementSource.isPlaying)
            return;

        MovementSource.clip = DeathClip;
        MovementSource.Play();
    }

    public void PlayDamageClip()
    {
        if(!DamageSource.isPlaying)
            DamageSource.Play();
    }

}
