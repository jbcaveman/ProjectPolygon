using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioClip roundSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip playButtonSound;

    [SerializeField] private AudioSource sfxSource;

    public void PlayClip(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;

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
}
