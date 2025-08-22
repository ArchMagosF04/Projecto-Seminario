using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float amount, Vector2 attackDirection);

    public void HealHealth(float amount);
}
