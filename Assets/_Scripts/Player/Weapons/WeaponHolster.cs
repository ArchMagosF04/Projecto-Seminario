using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour, IDataPersistance
{
    [SerializeField] WeaponList weaponList;
    public event Action<int> OnWeaponLoaded = delegate { };

    private int selectedWeapon;

    //[SerializeField]private bool manualOverride;
    //[SerializeField] private int manualWeaponOverrideIndex;

    public void LoadData(GameData gameData)
    {
        //if (!manualOverride)
        //{
            selectedWeapon = gameData.weaponSelected;
            OnWeaponLoaded(selectedWeapon);
        //}
        //else
        //{
        //    selectedWeapon = manualWeaponOverrideIndex;
        //    OnWeaponLoaded(selectedWeapon);
        //}
       
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
