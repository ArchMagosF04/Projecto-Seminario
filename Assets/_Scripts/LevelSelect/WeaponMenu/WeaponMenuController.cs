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

    private int currentSelectedWeapon;

    public void LoadData(GameData gameData)
    {
        currentSelectedWeapon = gameData.weaponSelected;
    }

    public void SaveData(GameData gameData)
    {
        gameData.weaponSelected = currentSelectedWeapon;
    }

    public void SetEquippedWeapon(int index)
    {
        currentSelectedWeapon = index;

        WeaponInfo weapon = weapons[currentSelectedWeapon];

        weaponImage.sprite = weapon.weaponImage;
        weaponName.text = weapon.weaponName;
        weaponDescription.text = weapon.weaponDescription;
    }

    public void GoToTrainingRoom()
    {
        AsyncSceneLoader.Instance.LoadLevel(weapons[currentSelectedWeapon].weaponTrainingRoomName);
    }
}
