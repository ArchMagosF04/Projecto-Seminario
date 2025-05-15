using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : WeaponComponent<WeaponMovementData, AttackMovement>
{
    private Movement coreMovement;
    private Movement CoreMovement => coreMovement ? coreMovement : core.GetCoreComponent(ref coreMovement);

    private void HandleStartMovement()
    {
        CoreMovement.SetVelocity(currentAttackData.Velocity, currentAttackData.Direction, CoreMovement.FacingDirection);
    }

    protected void HandleStopMovement()
    {
        CoreMovement.SetVelocityZero();
    }

    protected override void Start()
    {
        base.Start();

        EventHandler.OnStartMovement += HandleStartMovement;
        EventHandler.OnStopMovement += HandleStopMovement;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventHandler.OnStartMovement -= HandleStartMovement;
        EventHandler.OnStopMovement -= HandleStopMovement;
    }
}
