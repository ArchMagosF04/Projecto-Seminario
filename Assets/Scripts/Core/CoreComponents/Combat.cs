using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private float maxKnockbackTime = 0.2f;

    private bool isKnockbackActive;
    private float knockBackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged");
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        core.Movement.SetVelocity(strength, angle, direction);
        core.Movement.CanSetVelocity = false;
        isKnockbackActive = true;

        knockBackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if (isKnockbackActive && ((core.Movement.CurrentVelocity.y <= 0.01f && core.CollisionSenses.Grounded) || Time.time >= knockBackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            core.Movement.CanSetVelocity = true;
        }
    }
}
