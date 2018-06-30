using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSounds : MonoBehaviour {
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
        TowerAudioSource.Stop();
        PlayEventAudio(DestructionClip);
        Debug.Log("DESTRUCTIION");
    }

    public void PlayBuildingClip()
    {
        PlayEventAudio(BuildingClip);
    }

    public void PlayContiniousSource()
    {
        ContinousSource.Play();
    }

    public void PlayDamageSound()
    {
        DamageAudioSource.Play();
    }

    private void PlayEventAudio(AudioClip eventClip)
    {
        Debug.Log("PlayEventAudio");
        if (TowerAudioSource.isPlaying)
            return;

        TowerAudioSource.clip = eventClip;
        TowerAudioSource.Play();
    }

  
}
