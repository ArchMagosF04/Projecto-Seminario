using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AD_Damage 
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float Amount { get; private set; }
    [field: SerializeField, Range(1f, 3f)] public float OnBeatMultipler {  get; private set; }
}
