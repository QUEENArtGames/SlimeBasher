using UnityEngine;

public class TowerSounds : MonoBehaviour
{
    public AudioSource ContinousSource;

    public AudioSource TowerAudioSource;
    public AudioClip AttackClip;
    public AudioClip BuildingClip;
    public AudioClip DestructionClip;

    AudioSource DamageAudioSource;

    public void PlayAttackClip()
    {
        PlayEventAudio(AttackClip);
    }

    public void PlayDestructionClip()
    {
        PlayEventAudio(DestructionClip);
    }

    public void PlayBuildingClip()
    {
        PlayEventAudio(BuildingClip);
    }

    public void PlayDamageSound()
    {
        DamageAudioSource.Play();
    }

    private void PlayEventAudio(AudioClip eventClip)
    {
        TowerAudioSource.clip = eventClip;
        TowerAudioSource.Play();
    }
}
