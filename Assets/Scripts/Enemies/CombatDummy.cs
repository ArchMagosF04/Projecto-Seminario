using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour
{
    public Core Core { get; private set; }

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
    }

    private void Update()
    {
        Core.LogicUpdate();
    }
}
