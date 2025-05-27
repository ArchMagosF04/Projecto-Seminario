using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AD_Knockback
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Vector2 Angle { get; private set; }
    [field: SerializeField] public float Strength { get; private set; }
}
