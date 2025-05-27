using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon Data/Aggressive Weapon")]
public class SO_AggressiveWeaponData : SO_WeaponData
{
    [SerializeField] private WeaponAttackDetails[] attackDetails;
    public WeaponAttackDetails[] AttackDetails => attackDetails;

    private void OnEnable()
    {
        amountofAttacks = attackDetails.Length;

        movementSpeed = new float[amountofAttacks];

        for (int i = 0; i < amountofAttacks; i++)
        {
            movementSpeed[i] = attackDetails[i].movementSpeed;
        }
    }
}
