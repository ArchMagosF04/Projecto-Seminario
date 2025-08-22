using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Menu Panels")]
    [SerializeField] private Canvas pauseScreen;
    [SerializeField] private GameObject mainPauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject soundSettings;

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

    public void TogglePauseMenu(bool input)
    {
        IsGamePaused = input;

        if (input)
        {
            pauseScreen.enabled = true;
            BeatManager.Instance.ToggleMusic(false);
            Time.timeScale = 0f;
        }
        else
        {
            ResetScreens();
            Time.timeScale = 1f;
            BeatManager.Instance.ToggleMusic(true);
        }
    }

    public void ResetScreens()
    {
        pauseScreen.enabled = false;
        mainPauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        soundSettings.SetActive(false);
    }
}
