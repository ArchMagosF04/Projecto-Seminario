using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActionHitBox : WeaponComponent<WeaponActionHitboxData, AttackActionHitBox>
{
    private event Action<Collider2D[]> OnDetectedCollider2D;

    private CoreComp<Movement> movement;

    private Vector2 offset;

    private Collider2D[] detected;

    protected override void Start()
    {
        base.Start();

        movement = new CoreComp<Movement>(core);
    }

    private void HandleAttackAction()
    {
        offset.Set(
            transform.position.x + (currentAttackData.Hitbox.center.x * movement.Comp.FacingDirection),
            transform.position.y + currentAttackData.Hitbox.center.y);

        detected = Physics2D.OverlapBoxAll(offset, currentAttackData.Hitbox.size, 0f, data.DetectableLayers);

        if (detected.Length == 0) return;

        OnDetectedCollider2D?.Invoke(detected);

        foreach (var item in detected)
        {
            Debug.Log(item.name);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.OnAttackAction += HandleAttackAction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.OnAttackAction -= HandleAttackAction;
    }

    private void OnDrawGizmosSelected()
    {
        if(data == null) return;

        foreach (var item in data.AttackData)
        {
            if (!item.Debug) continue;

            Gizmos.DrawWireCube(transform.position + (Vector3)item.Hitbox.center, item.Hitbox.size);
        }
    }
}
