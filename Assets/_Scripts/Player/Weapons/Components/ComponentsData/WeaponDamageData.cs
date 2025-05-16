using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageData : ComponentData<AttackDamage>
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(WeaponDamage);
    }
}
