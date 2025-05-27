using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AD_Movement
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Vector2 Direction { get; private set; }
    [field: SerializeField] public float Velocity { get; private set; }
}
