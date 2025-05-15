using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageData : ComponentData<AttackDamage>
{
    public WeaponDamageData()
    {
        ComponentDependency = typeof(WeaponDamage);
    }
}
