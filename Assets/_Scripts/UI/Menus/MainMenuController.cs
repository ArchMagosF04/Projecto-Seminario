using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MenuPage startingPage;
    [SerializeField] private bool startOpen = true;

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
        Debug.Log("Exiting Application");
        Application.Quit();
    }
}
