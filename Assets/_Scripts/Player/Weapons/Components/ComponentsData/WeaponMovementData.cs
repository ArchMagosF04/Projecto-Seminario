using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementData : ComponentData<AttackMovement>
{
    public WeaponMovementData()
    {
        ComponentDependency = typeof(WeaponMovement);
    }
}
