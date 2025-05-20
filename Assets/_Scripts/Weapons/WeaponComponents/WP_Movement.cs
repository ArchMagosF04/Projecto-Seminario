using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_Movement : WeaponComponent
{
    private Movement coreMovement;
    private Movement CoreMovement => coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);

    private void HandleStartMovement()
    {
        AD_Movement data = WeaponData.MovementData[weapon.CurrentAttackCounter];

        CoreMovement.SetVelocity(data.Velocity, data.Direction, CoreMovement.FacingDirection);
    }

    private void HandleStopMovement()
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
