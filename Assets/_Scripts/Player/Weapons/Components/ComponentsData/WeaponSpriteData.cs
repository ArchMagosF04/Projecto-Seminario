using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteData : ComponentData<AttackSprites>
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(WeaponSprite);
    }
}
