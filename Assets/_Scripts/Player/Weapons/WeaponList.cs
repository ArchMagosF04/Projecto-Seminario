using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : ScriptableObject
{
    [SerializeField] private GameObject[] gameWeapons;

    public GameObject GetWeapon(int index)
    {
        return gameWeapons[index];
    }
}
