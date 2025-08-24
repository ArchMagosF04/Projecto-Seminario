using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private Canvas pauseScreen;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainPauseMenu;
    [SerializeField] private GameObject optionsMenu;

    [Header("Settings Panels")]
    [SerializeField] private GameObject volumeSettingsMenu;
    [SerializeField] private GameObject videoSettingsMenu;
    [SerializeField] private GameObject controlSettingsMenu;

    public bool IsGamePaused { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        IsGamePaused = false;
        ResetScreens();
    }

    public void ResetScreens()
    {
        pauseScreen.enabled = false;

        mainPauseMenu?.SetActive(true);
        optionsMenu?.SetActive(false);

        volumeSettingsMenu?.SetActive(false);
        videoSettingsMenu?.SetActive(false);
        controlSettingsMenu?.SetActive(false);
    }

    #region Main Pause Menu
    public void OpenPauseScreen()
    {
        IsGamePaused = true;

        pauseScreen.enabled = true;
        BeatManager.Instance.ToggleMusic(false);
        Time.timeScale = 0f;
    }

    public void ClosePauseScreen()
    {
        IsGamePaused = false;

        ResetScreens();
        Time.timeScale = 1f;
        BeatManager.Instance.ToggleMusic(true);
    }

    public void ResumeGame()
    {
        ClosePauseScreen();
    }

    public void QuitToMainMenu()
    {
        SceneLoaderManager.Instance.LoadSceneByIndex(0);
    }

    public void OpenOptionsMenu()
    {
        mainPauseMenu?.SetActive(false);
        optionsMenu?.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        mainPauseMenu?.SetActive(true);
        optionsMenu?.SetActive(false);
    }

    #endregion

    #region Options Menu

    public void OpenVolumeSettings()
    {
        optionsMenu?.SetActive(false);
        volumeSettingsMenu?.SetActive(true);
    }

    public void CloseVolumeSettings()
    {
        volumeSettingsMenu?.SetActive(false);
        optionsMenu?.SetActive(true);
    }

    public void OpenVideoSettings()
    {
        optionsMenu?.SetActive(false);
        videoSettingsMenu?.SetActive(true);
    }

    public void CloseVideoSettings()
    {
        videoSettingsMenu?.SetActive(false);
        optionsMenu?.SetActive(true);
    }

    public void OpenControlSettings()
    {
        optionsMenu?.SetActive(false);
        controlSettingsMenu?.SetActive(true);
    }

    public void CloseControlSettings()
    {
        controlSettingsMenu?.SetActive(false);
        optionsMenu?.SetActive(true);
    }

    #endregion
}
