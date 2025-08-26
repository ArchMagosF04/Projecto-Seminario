using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;

    [Header("Aditional Canvases")]
    [SerializeField] private Canvas volumeSettingsMenu;
    [SerializeField] private Canvas videoSettingsMenu;
    [SerializeField] private Canvas controlSettingsMenu;
    [SerializeField] private Canvas creditsMenu;

    [Header("Buttons to Select")]
    [SerializeField] private GameObject mainMenuFirstSelected;
    [SerializeField] private GameObject optionsMenuFirstSelected;
    [SerializeField] private GameObject volumeMenuFirstSelected;
    [SerializeField] private GameObject videoMenuFirstSelected;
    [SerializeField] private GameObject controlMenuFirstSelected;
    [SerializeField] private GameObject creditsFirstSelected;

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

        ResetScreens();
    }

    public void ResetScreens()
    {
        mainMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);

        volumeSettingsMenu.enabled = false;
        videoSettingsMenu.enabled = false;
        controlSettingsMenu.enabled = false;
        creditsMenu.enabled = false;

        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void OpenOptionsMenu()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(optionsMenuFirstSelected);
    }

    public void CloseOptionsMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void OpenCreditsScreen()
    {
        creditsMenu.enabled = true;

        EventSystem.current.SetSelectedGameObject(creditsFirstSelected);
    }

    public void CloseCreditsScreen()
    {
        ResetScreens();
    }

    #region Options Menu

    public void OpenVolumeSettings()
    {
        volumeSettingsMenu.enabled = true;

        EventSystem.current.SetSelectedGameObject(volumeMenuFirstSelected);
    }

    public void CloseVolumeSettings()
    {
        volumeSettingsMenu.enabled = false;

        EventSystem.current.SetSelectedGameObject(optionsMenuFirstSelected);
    }

    public void OpenVideoSettings()
    {
        videoSettingsMenu.enabled = true;

        EventSystem.current.SetSelectedGameObject(videoMenuFirstSelected);
    }

    public void CloseVideoSettings()
    {
        videoSettingsMenu.enabled = false;

        EventSystem.current.SetSelectedGameObject(optionsMenuFirstSelected);
    }

    public void OpenControlSettings()
    {
        controlSettingsMenu.enabled = true;

        EventSystem.current.SetSelectedGameObject(controlMenuFirstSelected);
    }

    public void CloseControlSettings()
    {
        controlSettingsMenu.enabled = false;

        EventSystem.current.SetSelectedGameObject(optionsMenuFirstSelected);
    }

    #endregion

    public void QuitGame()
    {
        Debug.Log("Closing Application");
        Application.Quit();
    }
}
