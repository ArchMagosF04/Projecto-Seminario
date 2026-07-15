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
    [SerializeField] private GameObject[] levelSprites;

    [Header("Menus")]
    [SerializeField] private MenuPage weaponSelectorCanvas;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopUpMenu confirmationPopUpMenu;

    [Header("Other")]
    [SerializeField] private EventSystem eventSystem;

    private int currentSelectedLevel;

    private bool isPopUpOpen;
    private bool isAnyMenuOpen;

    private bool lv1Unlocked;
    private bool lv2Unlocked;
    private bool lv3Unlocked;

    private void Awake()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
        isAnyMenuOpen = false;
        foreach (GameObject sprite in levelSprites) sprite.SetActive(false);
    }

    private void Start()
    {
        SelectButtonMarker();

        if (!lv1Unlocked) levelMarkers[1].interactable = false;
        else levelSprites[0].SetActive(true);
        if (!lv2Unlocked) levelMarkers[2].interactable = false;
        else levelSprites[1].SetActive(true);
        if (!lv3Unlocked) levelMarkers[3].interactable = false;
        else levelSprites[2].SetActive(true);
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
        if (isPopUpOpen)
        {
            return;
        }

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

            if (AnalyticsManager.Instance != null)
            {
                AnalyticsManager.Instance.SendWeaponSelectorOpenedEvent(
                    currentSelectedLevel,
                    GetAnalyticsLevelName(currentSelectedLevel),
                    GetSelectedWeaponAnalyticsId()
                );
            }
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
        string selectedSceneName =
            levelNames[currentSelectedLevel];

        LevelAttemptTracker.ResetAttemptsForScene(
            selectedSceneName
        );

        AsyncSceneLoader.Instance.LoadLevel(
            selectedSceneName
        );
    }

    public void LoadData(GameData gameData)
    {
        currentSelectedLevel = gameData.lastLevelSelected;
        lv1Unlocked = gameData.unlocked1stLevel;
        lv2Unlocked = gameData.unlocked2ndLevel;
        lv3Unlocked = gameData.unlocked3rdLevel;
    }

    public void SaveData(GameData gameData)
    {
        gameData.lastLevelSelected = currentSelectedLevel;
    }

    private string GetAnalyticsLevelName(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0:
                return "tutorial";

            case 1:
                return "argentina";

            case 2:
                return "brazil";

            case 3:
                return "mexico";

            default:
                return "unknown";
        }
    }

    private string GetSelectedWeaponAnalyticsId()
    {
        if (DataPersistanceManager.Instance == null ||
            !DataPersistanceManager.Instance.HasGameData())
        {
            return "unknown";
        }

        int weaponIndex =
            DataPersistanceManager.Instance.GetSelectedWeapon();

        switch (weaponIndex)
        {
            case 0:
                return "microphone";

            case 1:
                return "accordion";

            case 2:
                return "saxophone";

            case 3:
                return "weapon_4";

            default:
                return "unknown";
        }
    }
}
