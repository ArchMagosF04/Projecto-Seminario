using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_FireProjectile : WeaponComponent
{
    [SerializeField] private ExplosiveProjectile bomb;

    private Movement coreMovement;
    private Movement CoreMovement => coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);

    protected override void OnEnable()
    {
        base.OnEnable();

        EventHandler.OnFireProjectile += FireProjectile;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventHandler.OnFireProjectile -= FireProjectile;
    }

    private void FireProjectile()
    {
        ExplosiveProjectile newBomb = Instantiate(bomb, transform.position, Quaternion.identity);
        newBomb.FireProjectile(CoreMovement.FacingDirection);
    }
}
