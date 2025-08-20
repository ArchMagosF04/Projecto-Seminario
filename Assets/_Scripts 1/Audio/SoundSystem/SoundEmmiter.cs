using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmmiter : MonoBehaviour
{
    public SoundData Data { get; private set; }

    private AudioSource audioSource;
    private Coroutine playingCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (playingCoroutine != null) StopCoroutine(playingCoroutine);

        audioSource.Play();
        playingCoroutine = StartCoroutine(WaitForSoundToEnd());
    }

    private IEnumerator WaitForSoundToEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        SoundManager.Instance.ReturnToPool(this);
    }

    public void Stop()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
            playingCoroutine = null;
        }

        audioSource.Stop();
        SoundManager.Instance.ReturnToPool(this);
    }

    public void WithRandomPitch(float min, float max)
    {
        audioSource.pitch = 1f;
        audioSource.pitch += Random.Range(min, max);
    }

    public void Initialize(SoundData data)
    {
        Data = data; 
        audioSource.clip = data.Clip;
        audioSource.outputAudioMixerGroup = data.MixerGroup;
        audioSource.loop = data.Loop;
        audioSource.playOnAwake = data.PlayOnAwake;
    }
}
