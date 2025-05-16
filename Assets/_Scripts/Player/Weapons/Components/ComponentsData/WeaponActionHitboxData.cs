using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActionHitboxData : ComponentData<AttackActionHitBox>
{
    [field: SerializeField] public LayerMask DetectableLayers {  get; private set; }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(WeaponActionHitBox);
    }
}
