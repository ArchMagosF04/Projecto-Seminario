using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackReceiver : CoreComponent, IKnockbackable
{
    [SerializeField] private float maxKnockBackTime = 0.2f;

    private bool isKnockBackActive;
    private float knockBackStartTime;

    private Movement movement;
    private CollisionSenses collisionSenses;

    protected override void Awake()
    {
        base.Awake();

        movement = core.GetCoreComponent<Movement>();
        collisionSenses = core.GetCoreComponent<CollisionSenses>();
    }

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        movement.SetVelocity(strength, angle, direction);
        movement.CanSetVelocity = false;
        isKnockBackActive = true;

        knockBackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if (isKnockBackActive && ((movement.CurrentVelocity.y <= 0.01f && collisionSenses.Grounded) || Time.time >= knockBackStartTime + maxKnockBackTime))
        {
            isKnockBackActive = false;
            movement.CanSetVelocity = true;
        }
    }
}
