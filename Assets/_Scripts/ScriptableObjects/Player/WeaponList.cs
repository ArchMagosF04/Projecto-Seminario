using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newWeaponList", menuName = "Data/WeaponList")]
public class WeaponList : ScriptableObject
{
    [SerializeField] private GameObject[] gameWeapons;

    public GameObject GetWeapon(int index)
    {
        return gameWeapons[index];
    }
}
