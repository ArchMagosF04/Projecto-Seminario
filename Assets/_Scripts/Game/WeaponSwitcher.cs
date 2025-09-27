using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private GlobalManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GlobalManager.Instance;
    }

    public void ChangeWeapon(int weapon)
    {
        gManager.SetCurrentWeapon(weapon);
    }

}
