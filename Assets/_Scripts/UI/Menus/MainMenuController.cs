using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MenuPage startingPage;
    [SerializeField] private bool startOpen = true;
    [SerializeField] private ConfirmationPopUpMenu popUpMenu;

    private MenuPage currentPage;

    private void Awake()
    {
        if (startingPage == null) Debug.LogError("Starting page Not Selected", this);
        currentPage = startingPage;
    }

    private void Start()
    {
        if (startOpen) startingPage.OpenMenu();
    }

    private void Update()
    {
        if (UIInputManager.Instance.CancelMenuInput)
        {
            UIInputManager.Instance.UseCancelMenuInput();
            GoToPreviousMenu();
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

    public void QuitApplication()
    {

        popUpMenu.ActivateMenu("Are you sure you want to Quit?",
            () =>
            {
                currentPage.ChangeObjectSelected();
                Debug.Log("Exiting Application");
                Application.Quit();
            },
            () =>
            {
                currentPage.ChangeObjectSelected();
            }
        );
    }
}
