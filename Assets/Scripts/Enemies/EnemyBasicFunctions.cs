using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicFunctions : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField]private float hp;
    private float spd;
    private float atk;
    [SerializeField] private bool useOwnGracePeriod;
    [SerializeField] private float gracePeriodForBeat;
    private float graceTimer;    

    public static bool onBeat;
   
    void Start()
    {
        hp = stats.Hp;
        spd = stats.Spd;
        atk = stats.Atk;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            Debug.Log(damage);
        }
        
    }   
        
}
