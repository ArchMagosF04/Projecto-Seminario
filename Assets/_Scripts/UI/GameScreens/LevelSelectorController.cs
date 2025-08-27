using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Level Lists")]
    [SerializeField] private Transform[] locationsTransform;
    [SerializeField] private string[] levelNames;

    [Header("UI Panels")]
    [SerializeField] private GameObject weaponPanel;

    [Header("UI Selection")]
    [SerializeField] private GameObject weaponPanelFirstSelected;

    private int levelIndex;
    private bool isWeaponMenuOpen;


    private void Awake()
    {
        weaponPanel.SetActive(false);

        isWeaponMenuOpen = false;
        levelIndex = 0;
        virtualCamera.Follow = locationsTransform[0];
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
        if (levelIndex >= locationsTransform.Length - 1 || isWeaponMenuOpen) return;

        levelIndex++;
        virtualCamera.Follow = locationsTransform[levelIndex];
    }

    public void SelectThePreviousLevel()
    {
        if (levelIndex <= 0 || isWeaponMenuOpen) return;

        levelIndex--;
        virtualCamera.Follow = locationsTransform[levelIndex];
    }

    public void ConfirmLevelSelection()
    {
        if (isWeaponMenuOpen || levelNames[levelIndex] == null || levelNames[levelIndex] == "null") return;

        isWeaponMenuOpen = true;

        weaponPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(weaponPanelFirstSelected);
    }

    public void CancelWeaponSelection()
    {
        if (!isWeaponMenuOpen) return;

        isWeaponMenuOpen = false;

        weaponPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadSelectedLevel()
    {
        SceneLoaderManager.Instance.LoadSceneByName(levelNames[levelIndex]);
    }
}
