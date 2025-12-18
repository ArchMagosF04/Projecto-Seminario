using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private MenuPage startingPage;
    [SerializeField] private ConfirmationPopUpMenu popUpMenu;
    [SerializeField] private Canvas backgroundCanvas;

    private MenuPage currentPage;

    public bool IsGameInPause { get; private set; }

    private PlayerInput playerInput;

    private void Awake()
    {
        if (startingPage == null) Debug.LogError("Starting page Not Selected", this);
        currentPage = startingPage;

        backgroundCanvas.enabled = false;
    }

    private void Start()
    {
        playerInput = GameInputManager.Instance.gameObject.GetComponent<PlayerInput>();
    }

    private void OnDisable()
    {
        ResumeTimeScale();
    }

    private void Update()
    {
        if (!IsGameInPause && GameManager.Instance.IsGameActive)
        {
            if (GameInputManager.Instance.PauseInput)
            {
                GameInputManager.Instance.UsePauseInput();
                OpenPauseMenu();
            }
        }

        if (IsGameInPause)
        {
            if (UIInputManager.Instance.CancelMenuInput)
            {
                UIInputManager.Instance.UseCancelMenuInput();
                GoToPreviousMenu();
            }

            if (UIInputManager.Instance.ResumeInput)
            {
                UIInputManager.Instance.UseResumeInput();
                ClosePauseMenu();
            }
        }
    }

    public void GoToPreviousMenu()
    {
        MenuPage menu = currentPage.GoToPreviousPage();
        if (menu != null) currentPage = menu;
    }

    public void GoToMenu(MenuPage menu)
    {
        currentPage.CloseMenu();
        menu.OpenMenu();
        currentPage = menu;
    }

    public void OnStartInput()
    {
        if (IsGameInPause) ClosePauseMenu();
        else OpenPauseMenu();
    }

    public void OpenPauseMenu()
    {
        playerInput.SwitchCurrentActionMap("UI");
        IsGameInPause = true;
        BeatManager.Instance.ToggleMusic(false);
        PauseTimeScale();
        currentPage = startingPage;
        currentPage.OpenMenu();
        backgroundCanvas.enabled = true;
    }

    public void ClosePauseMenu()
    {
        ResumeTimeScale();
        BeatManager.Instance.ToggleMusic(true);
        currentPage.CloseMenu();
        IsGameInPause = false;
        playerInput.SwitchCurrentActionMap("Player");
        backgroundCanvas.enabled = false;
    }

    public void PauseTimeScale() => Time.timeScale = 0f;
    public void ResumeTimeScale() => Time.timeScale = 1f;

    public void GoToMainMenuButton()
    {
        MenuPage page = currentPage;
        currentPage = null;

        popUpMenu.ActivateMenu("Are you sure you want to return to the Main Manu?",
            () =>
            {
                AsyncSceneLoader.Instance.LoadLevel(0);
            },
            () =>
            {
                currentPage = page;
                currentPage.ChangeObjectSelected();
            }
        );
    }

    public void ExitLevel()
    {
        MenuPage page = currentPage;
        currentPage = null;

        popUpMenu.ActivateMenu("Are you sure you want to exit the Level?",
            () =>
            {
                AsyncSceneLoader.Instance.LoadLevel(1);
            },
            () =>
            {
                currentPage = page;
                currentPage.ChangeObjectSelected();
            }
        );
    }
}
