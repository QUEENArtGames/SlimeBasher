using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour {

    public AudioSource GameSoundSource;
    public AudioClip RoundStartClip;
    public AudioClip RoundEndClip;

    public void PlayRoundStartClip()
    {
        GameSoundSource.clip = RoundStartClip;
        GameSoundSource.Play();
    }

    public void PlayRoundEndClip()
    {
        GameSoundSource.clip = RoundEndClip;
        GameSoundSource.Play();
    }
}
