using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [field: SerializeField] public Stat Health {  get; private set; }
    protected override void Awake()
    {
        base.Awake();

        Health.Init();
    }
}
