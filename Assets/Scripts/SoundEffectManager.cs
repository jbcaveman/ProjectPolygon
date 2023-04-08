using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioClip roundSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    [SerializeField] private AudioSource sfxSource;

    /// Use this method for short clips with volume control & 3D sound
    public void PlayClip(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;

        Debug.Log($"PlayOnce: {clip.name}, at {volume}");

        AudioSource audioSource = CreateAudioSourceWith(clip, volume);
        audioSource.Play();

        Destroy(audioSource.gameObject, clip.length);
    }

    private AudioSource CreateAudioSourceWith(AudioClip clip, float volume)
    {
        AudioSource audioSource = Instantiate(sfxSource, new(0,0,0), Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = false;
        return audioSource;
    }

    public void StopPlaying(AudioSource source)
    {
        source.Stop();
        Destroy(source.gameObject);
    }
}
