using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectPannel : MonoBehaviour
{
    [SerializeField] LvlSelectionController controller;
    [SerializeField] GameObject weaponSelectionPanel;
    [SerializeField] GameObject[] weaponButtons;
    private int availableWeapons = 1;

    private bool buttonsHidden = false;
    private bool buttonsShown = false;

    // Start is called before the first frame update
    void Start()
    {
        LvlSelectionController.OnSelection += ToggleWeaponPanel;
        DeactivateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponSelectionPanel.activeSelf && !buttonsShown)
        {
            ActivateButtons();
            buttonsShown = true;
        }
       
    }

    private void ActivateButtons()
    {
        //availableWeapons = GameManager.GetAvailableWeapons();

        for (int i =0;  i < availableWeapons; i++)
        {
            weaponButtons[i].SetActive(true);
        }
    }

    private void DeactivateButtons()
    {
        foreach (var weaponButton in weaponButtons)
        {
            if (weaponButton != null)
            {
                weaponButton.gameObject.SetActive(false);
            }
        }
    }

    public void EquipSelectedWeapon(int weapon)
    {
        //GameManager.EquipWeapon(weapon);
    }

    private void ToggleWeaponPanel(bool toggle)
    {
        weaponSelectionPanel.SetActive(toggle);
    }
}
