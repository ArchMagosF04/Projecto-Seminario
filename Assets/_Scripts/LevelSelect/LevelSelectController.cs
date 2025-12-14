using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour, IDataPersistance
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Marker Settings")]
    [SerializeField] private Selectable[] levelMarkers;

    [Header("Level Names")]
    [SerializeField] private string[] levelNames;

    [Header("Menus")]
    [SerializeField] private MenuPage weaponSelectorCanvas;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopUpMenu confirmationPopUpMenu;

    [Header("Other")]
    [SerializeField] private EventSystem eventSystem;

    private int currentSelectedLevel;

    private bool isPopUpOpen;
    private bool isAnyMenuOpen;

    private void Awake()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
        isAnyMenuOpen = false;
    }

    private void Start()
    {
        SelectButtonMarker();
    }

    public void OnReturnToMainMenuInput()
    {
        if (weaponSelectorCanvas.IsMenuOpen) return;

        isAnyMenuOpen = true;
        isPopUpOpen = true;

        confirmationPopUpMenu.ActivateMenu("Are you sure you want to return to the main menu?",
            () =>
            {
                AsyncSceneLoader.Instance.LoadLevel(0);
            },
            () =>
            {
                CloseMenu();
            }
        );
    }

    public void OpenWeaponsMenu()
    {
        if (isPopUpOpen) return;

        if (weaponSelectorCanvas.IsMenuOpen)
        {
            isAnyMenuOpen = false;
            weaponSelectorCanvas.CloseMenu();
            SelectButtonMarker();
        }
        else
        {
            isAnyMenuOpen = true;
            weaponSelectorCanvas.OpenMenu();
        }
    }

    public void SelectButtonMarker()
    {
        if (eventSystem == null) Debug.Log("No event system reference", this);

        eventSystem.SetSelectedGameObject(levelMarkers[currentSelectedLevel].gameObject);
        virtualCamera.Follow = levelMarkers[currentSelectedLevel].gameObject.transform;
    }

    public void SelectNextMarker()
    {
        if (currentSelectedLevel < levelMarkers.Length - 1 && !isAnyMenuOpen)
        {
            currentSelectedLevel++;
            SelectButtonMarker();
        }
    }

    public void SelectPreviousMarker()
    {
        if (currentSelectedLevel > 0 && !isAnyMenuOpen)
        {
            currentSelectedLevel--;
            SelectButtonMarker();
        }
    }

    public void CloseMenu()
    {
        isAnyMenuOpen = false;
        isPopUpOpen = false;
        weaponSelectorCanvas.CloseMenu();
        confirmationPopUpMenu.DeactivateMenu();
        SelectButtonMarker();
    }

    public void PlaySelectedLevel()
    {
        AsyncSceneLoader.Instance.LoadLevel(levelNames[currentSelectedLevel]);
    }

    public void LoadData(GameData gameData)
    {
        currentSelectedLevel = gameData.lastLevelSelected;
    }

    public void SaveData(GameData gameData)
    {
        gameData.lastLevelSelected = currentSelectedLevel;
    }
}
