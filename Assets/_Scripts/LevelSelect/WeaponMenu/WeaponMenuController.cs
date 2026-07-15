using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenuController : MonoBehaviour, IDataPersistance
{
    [Header("Weapons Data")]
    [SerializeField] private List<WeaponInfo> weapons;

    [Header("Weapon Details Box")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponName;
    [SerializeField] private TMP_Text weaponDescription;

    [Header("Weapon Selected Highlights")]
    [SerializeField] private List<GameObject> highlights;

    [Header("Weapon Locks")]
    [SerializeField] private Button weapon2Button;
    [SerializeField] private GameObject weapon2Lock;
    [SerializeField] private Button weapon3Button;
    [SerializeField] private GameObject weapon3Lock;

    private int currentSelectedWeapon;

    private bool weapon2Unlocked;
    private bool weapon3Unlocked;

    private void Awake()
    {
        foreach (var img in highlights) img.SetActive(false);
    }

    private void Start()
    {
        WeaponInfo weapon = weapons[currentSelectedWeapon];

        highlights[currentSelectedWeapon].SetActive(true);

        weaponImage.sprite = weapon.weaponImage;
        weaponName.text = weapon.weaponName;
        weaponDescription.text = weapon.weaponDescription;

        if (!weapon2Unlocked)
        {
            weapon2Button.interactable = false;
            weapon2Lock.SetActive(true);
        }
        else
        {
            weapon2Button.interactable = true;
            weapon2Lock.SetActive(false);
        }

        if (!weapon3Unlocked)
        {
            weapon3Button.interactable = false;
            weapon3Lock.SetActive(true);
        }
        else
        {
            weapon3Button.interactable = true;
            weapon3Lock.SetActive(false);
        }
    }

    public void LoadData(GameData gameData)
    {
        currentSelectedWeapon = gameData.weaponSelected;
        weapon2Unlocked = gameData.unlockedweapon2;
        weapon3Unlocked = gameData.unlockedweapon3;
    }

    public void SaveData(GameData gameData)
    {
        gameData.weaponSelected = currentSelectedWeapon;
    }

    public void SetEquippedWeapon(int index)
    {
        int previousIndex = currentSelectedWeapon;

        string previousWeaponId =
            GetWeaponAnalyticsId(previousIndex);

        currentSelectedWeapon = index;

        string selectedWeaponId =
            GetWeaponAnalyticsId(currentSelectedWeapon);

        WeaponInfo weapon = weapons[currentSelectedWeapon];

        foreach (var img in highlights)
        {
            img.SetActive(false);
        }

        highlights[currentSelectedWeapon].SetActive(true);

        weaponImage.sprite = weapon.weaponImage;
        weaponName.text = weapon.weaponName;
        weaponDescription.text = weapon.weaponDescription;

        if (DataPersistanceManager.Instance != null &&
            DataPersistanceManager.Instance.HasGameData())
        {
            DataPersistanceManager.Instance.ChangeSelectedWeapon(index);
            DataPersistanceManager.Instance.SaveGame();
        }

        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendWeaponSelectedEvent(
                selectedWeaponId,
                currentSelectedWeapon,
                previousWeaponId,
                previousIndex != currentSelectedWeapon
            );
        }

        Debug.Log(
            $"ARMA SELECCIONADA | Índice: {index} | Nombre: {weapon.weaponName}"
        );
    }

    public void GoToTrainingRoom()
    {
        AsyncSceneLoader.Instance.LoadLevel(weapons[currentSelectedWeapon].weaponTrainingRoomName);
    }

    private string GetWeaponAnalyticsId(int index)
    {
        switch (index)
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
