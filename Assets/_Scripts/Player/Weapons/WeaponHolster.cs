using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    [SerializeField] WeaponList weaponList;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(weaponList.GetWeapon(GlobalManager.Instance.selectedWeapon), this.transform.parent);
        Destroy(this.gameObject);
    }

    
}
