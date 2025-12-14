using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour, IDataPersistance
{
    [SerializeField] WeaponList weaponList;

    private int selectedWeapon;

    public void LoadData(GameData gameData)
    {
        selectedWeapon = gameData.weaponSelected;
    }

    public void SaveData(GameData gameData)
    {
        
    }

    private void Awake()
    {
        GameObject temp = null;
        temp = Instantiate(weaponList.GetWeapon(selectedWeapon), this.transform.parent);
        this.transform.parent.GetComponent<PlayerController>().weapon = temp.GetComponent<PlayerWeapon>();
        Destroy(this.gameObject);
    }
}
