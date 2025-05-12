using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount)
    {
        Debug.Log(amount + " Damage Taken");
    }
}
