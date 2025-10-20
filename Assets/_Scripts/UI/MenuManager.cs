using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private Menu[] menus;
    [SerializeField] private Menu initialMenu;
    [SerializeField] private bool startWithMenuOpen = false;

    private Menu currentMenu;

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
    }

    private void Start()
    {
        if (startWithMenuOpen)
        {
            if (initialMenu == null) initialMenu = menus[0];
            OpenMenu(initialMenu);
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].MenuName == menuName)
            {
                menus[i].Open();
                currentMenu = menus[i];
            }
            else if (menus[i].IsOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].IsOpen)
            {
                CloseMenu(menus[i]);
            }
        }

        menu.Open();
        currentMenu = menu;
    }

    public void OpenPreviousMenu()
    {
        currentMenu.Close();
        currentMenu = currentMenu.previousMenu;
        currentMenu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
