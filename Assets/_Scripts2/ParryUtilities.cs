using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParryMethods
{
    public static class ParryUtilities
    {
        public static Vector2 ReverseAttack(Vector2 atkDir)
        {
            return -atkDir;
        }

        /// <summary>
        /// 13 = EnemyAttack, 14 = PlayerAttack. Default is 14
        /// </summary>
        public static void SwitchAttackLayer(GameObject projectile, int newLayer = 14)
        {
            projectile.layer = newLayer;
        }
    }
}


