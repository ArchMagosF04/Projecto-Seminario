using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AsyncSceneLoader asyncLoader; 

    [Header("Level Lists")]
    [SerializeField] private Transform[] locationsTransform;
    [SerializeField] private string[] levelNames;

    [Header("UI Panels")]
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private GameObject lockedMessage;
    [SerializeField] private GameObject comingSoonMessage;

    [Header("UI Selection")]
    [SerializeField] private GameObject weaponPanelFirstSelected;

    [Header("Behaviour")]
    [SerializeField] private int startingIndex = 1;
    [SerializeField] private int minSelectableIndex = 1;

    private int levelIndex;
    private bool isWeaponMenuOpen;

    private void Awake()
    {
        SetStartingIndex(PlayerPrefs.GetInt("CompletedLevels"));
        weaponPanel.SetActive(false);
        lockedMessage.SetActive(false);
        comingSoonMessage.SetActive(false);
        isWeaponMenuOpen = false;
        levelIndex = Mathf.Clamp(startingIndex, 0, locationsTransform.Length - 1);
        virtualCamera.Follow = locationsTransform[levelIndex];
    }

    private void Start()
    {
        InputManager.Instance.OnMainMenu += GoBackToMainMenu;
        InputManager.Instance.OnNextLevel += SelectTheNextLevel;
        InputManager.Instance.OnPreviousLevel += SelectThePreviousLevel;
        InputManager.Instance.OnSelectLevel += ConfirmLevelSelection;
        InputManager.Instance.OnCloseWeaponScreen += CancelWeaponSelection;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMainMenu -= GoBackToMainMenu;
        InputManager.Instance.OnNextLevel -= SelectTheNextLevel;
        InputManager.Instance.OnPreviousLevel -= SelectThePreviousLevel;
        InputManager.Instance.OnSelectLevel -= ConfirmLevelSelection;
        InputManager.Instance.OnCloseWeaponScreen -= CancelWeaponSelection;
    }

    public void GoBackToMainMenu()
    {
        SceneLoaderManager.Instance.LoadSceneByIndex(0);
    }

    public void SelectTheNextLevel()
    {
        if (isWeaponMenuOpen) return;
        if (levelIndex >= locationsTransform.Length - 1) return;
        levelIndex++;
        virtualCamera.Follow = locationsTransform[levelIndex];
    }

    public void SelectThePreviousLevel()
    {
        if (isWeaponMenuOpen) return;
        if (levelIndex - 1 < 0) return;
        levelIndex--;
        virtualCamera.Follow = locationsTransform[levelIndex];
    }

    public void ConfirmLevelSelection()
    {
        if (isWeaponMenuOpen) return;
        if (levelNames[levelIndex] == "null")
        {
            comingSoonMessage.SetActive(true);
            return;
        }
        if ((levelIndex > 0 && PlayerPrefs.GetInt("CompletedTutorial") != 1) || PlayerPrefs.GetInt("CompletedLevels") + 1 < levelIndex)
        {
            lockedMessage.SetActive(true);
            return;
        }
        if (levelIndex < Mathf.Clamp(minSelectableIndex, 0, locationsTransform.Length - 1)) return;
        var name = levelNames != null && levelIndex < levelNames.Length ? levelNames[levelIndex] : null;
        if (string.IsNullOrWhiteSpace(name) || name == "null") return;
        isWeaponMenuOpen = true;
        weaponPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(weaponPanelFirstSelected);
    }

    public void SetStartingIndex(int value)
    {
        startingIndex = value;
    }

    public void CancelWeaponSelection()
    {
        if (!isWeaponMenuOpen) return;
        isWeaponMenuOpen = false;
        weaponPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void CloseLockedWindow()
    {
        lockedMessage.SetActive(false);
        comingSoonMessage.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadSelectedLevel()
    {
        //SceneLoaderManager.Instance.LoadSceneByName(levelNames[levelIndex]);
        asyncLoader.LoadLevel(levelNames[levelIndex]);

    }

    public string GetSelectedLevel()
    {
        return levelNames[levelIndex];
    }

    public int GetSelectedLevelIdex()
    {
        return levelIndex;
    }
}
