using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_Movement : WP_Component
{
    private Movement coreMovement;
    private Movement CoreMovement => coreMovement ? coreMovement : core.GetCoreComponent(ref coreMovement);

    private void HandleStartMovement()
    {
        //CoreMovement.SetVelocity(weapon.currentAttackDetails.movementSpeed, weapon.currentAttackDetails.angle, CoreMovement.FacingDirection);
    }

    protected void HandleStopMovement()
    {
        CoreMovement.SetVelocityZero();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventHandler.OnStartMovement += HandleStartMovement;
        EventHandler.OnStopMovement += HandleStopMovement;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventHandler.OnStartMovement -= HandleStartMovement;
        EventHandler.OnStopMovement -= HandleStopMovement;
    }
}
