using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    [SerializeField] WeaponList weaponList;


    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = null;
        temp = Instantiate(weaponList.GetWeapon(GlobalManager.Instance.selectedWeapon), this.transform.parent);
        this.transform.parent.GetComponent<PlayerController>().weapon = temp.GetComponent<PlayerWeapon>();
        Destroy(this.gameObject);
    }

    
}
