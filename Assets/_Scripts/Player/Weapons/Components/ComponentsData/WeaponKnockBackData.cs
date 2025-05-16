using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnockBackData : ComponentData<AttackKnockBack>
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(WeaponKnockBack);
    }
}
