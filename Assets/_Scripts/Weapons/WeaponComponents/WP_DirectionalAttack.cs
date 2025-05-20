using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_DirectionalAttack : WeaponComponent
{
    private Vector2 workSpace;

    private Vector2[] directions;

    protected override void Awake()
    {
        base.Awake();

        directions = new Vector2[5];

        directions[0] = new Vector2(0, 0); //Forward
        directions[1] = new Vector2(0, 1); //Upward
        directions[2] = new Vector2(0, -1); //Downward
        directions[3] = new Vector2(1, 1); //Up-Diagonal
        directions[4] = new Vector2(1, -1); //Down-Diagonal
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        weapon.OnEnter += HandleDirection;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        weapon.OnEnter += HandleDirection;
    }

    private void HandleDirection()
    {
        weapon.ModifyAttackCounter(CheckDirection());
    }

    private int CheckDirection()
    {
        workSpace.Set(Mathf.Abs(weapon.state.XInput), weapon.state.YInput);

        for (int i = 0; i < directions.Length; i++)
        {
            if (directions[i] == workSpace)
            {
                //Debug.Log(i);
                return i;
            }
        }

        return 0;
    }
}
