using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    [SerializeField] private string name;

    public void SetAttackName(int i) => name = $"Attack {i}";
}
