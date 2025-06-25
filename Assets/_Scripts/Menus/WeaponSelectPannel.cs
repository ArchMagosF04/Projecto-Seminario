using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectPannel : MonoBehaviour
{
    [SerializeField] LvlSelectionController controller;
    [SerializeField] GameObject weaponSelectionPanel;
    [SerializeField] GameObject[] weaponButtons;
    private int availableWeapons = 1;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.LevelSelected)
        {
            weaponSelectionPanel.SetActive(true);
        }
    }

    private void OnEnable()
    {
        availableWeapons = GameManager.GetAvailableWeapons().Count;

        for (int i =0;  i < availableWeapons; i++)
        {
            weaponButtons[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (var weaponButton in weaponButtons)
        {
            weaponButton.gameObject.SetActive(false);
        }
    }

    public void EquipSelectedWeapon(int weapon)
    {
        GameManager.EquipWeapon(weapon);
    }
}
