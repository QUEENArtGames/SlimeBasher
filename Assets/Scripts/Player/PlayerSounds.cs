﻿using SlimeBasher.Characters.ThirdPerson;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioSource VoiceSource;
    public AudioClip[] CheerClips;
    public AudioClip[] PainClips;
    public AudioClip[] KoClipts;

    public AudioSource AttackSource;
    public AudioClip[] AttackClips;
    public AudioSource StepSource;

    public AudioSource InventoryCollectionSource;

    private ThirdPersonCharacter _thirdPersonCharacter;

    private void Start()
    {
        _thirdPersonCharacter = FindObjectOfType<ThirdPersonCharacter>();
    }
    public void PlayKoSound()
    {
        VoiceSource.Stop();
        PlayVoiceClip(KoClipts[(int) Random.Range(0, KoClipts.Length)]);
    }

    public void PlayPainSound()
    {
        PlayVoiceClip(PainClips[(int) Random.Range(0, PainClips.Length)]);
    }

    public void PlayCheerSound()
    {
        PlayVoiceClip(CheerClips[(int) Random.Range(0, CheerClips.Length)]);
    }

    public void PlayAttackSound()
    {
        AttackSource.clip = AttackClips[(int) Random.Range(0, AttackClips.Length)];
        AttackSource.Play();
    }

    public void PlayCollectionSound()
    {
        InventoryCollectionSource.Play();
    }

    public void PlayFootstepSound()
    {
        if (_thirdPersonCharacter.IsGrounded)
            StepSource.Play();
    }

    private void PlayVoiceClip(AudioClip voiceClip)
    {
        if (VoiceSource.isPlaying)
            return;

        VoiceSource.clip = voiceClip;
        VoiceSource.Play();
    }
}
