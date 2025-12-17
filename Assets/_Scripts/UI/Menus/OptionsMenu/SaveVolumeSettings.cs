using Ami.BroAudio;
using Ami.BroAudio.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SaveVolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider uISlider;

    private float savedUIVolume;

    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        SoundManager.Init();

        savedUIVolume = PlayerPrefs.GetFloat("SavedUIVolume", 1);
        SetUIVolume(0);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
        uISlider.onValueChanged.AddListener(SetUIVolume);
    }

    private void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 1));
        SetMusicVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 1));
        SetSoundVolume(PlayerPrefs.GetFloat("SavedSoundVolume", 1));
        StartCoroutine(SetUIVolume());
    }

    private IEnumerator SetUIVolume()
    {
        yield return new WaitForSeconds(0.5f);
        SetUIVolume(savedUIVolume);
    }

    private void OnDisable()
    {
        masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        soundSlider.onValueChanged.RemoveListener(SetSoundVolume);
        uISlider.onValueChanged.RemoveListener(SetUIVolume);
    }

    public void SetMasterVolume(float volume)
    {
        RefreshSlider(volume, masterSlider);

        PlayerPrefs.SetFloat("SavedMasterVolume", volume);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        RefreshSlider(volume, musicSlider);

        PlayerPrefs.SetFloat("SavedMusicVolume", volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume / 100) * 20);
    }

    public void SetSoundVolume(float volume)
    {
        RefreshSlider(volume, soundSlider);

        PlayerPrefs.SetFloat("SavedSoundVolume", volume);
    }

    public void SetUIVolume(float volume)
    {
        RefreshSlider(volume, uISlider);

        PlayerPrefs.SetFloat("SavedUIVolume", volume);
    }

    public void RefreshSlider(float volume, Slider slider)
    {
        slider.value = volume;
    }
}
