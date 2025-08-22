using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core_Knockback : CoreComponent
{
    [SerializeField] private float maxKnockBackTime = 0.2f;

    public bool HyperArmor { get; private set; }

    private bool isKnockBackActive;
    private float knockBackStartTime;

    private Core_Movement movement;
    private Core_CollisionSenses collisionSenses;

    protected override void Awake()
    {
        base.Awake();

        movement = core.GetCoreComponent<Core_Movement>();
        collisionSenses = core.GetCoreComponent<Core_CollisionSenses>();
    }

    public override void LogicUpdate()
    {
        CheckKnockBack();
    }

    public void ToggleHyperArmor(bool value)
    {
        HyperArmor = value;
    }

    public void Knockback(Transform origin, float strength)
    {
        if (HyperArmor) return;

        Vector2 direction = transform.position - origin.position;

        movement.SetVelocity(strength, direction.normalized);
        movement.CanSetVelocity = false;
        isKnockBackActive = true;

        knockBackStartTime = Time.time;
    }

    public void KnockBack(Vector2 angle, float strength, int direction)
    {
        if (HyperArmor) return;

        movement.SetVelocity(strength, angle, direction);
        movement.CanSetVelocity = false;
        isKnockBackActive = true;

        knockBackStartTime = Time.time;
    }

    private void CheckKnockBack()
    {
        if (isKnockBackActive && (/*(movement.CurrentVelocity.y <= 0.01f && collisionSenses.Grounded) || */Time.time >= knockBackStartTime + maxKnockBackTime))
        {
            isKnockBackActive = false;
            movement.CanSetVelocity = true;
            movement.SetVelocityZero();
        }
    }
}
