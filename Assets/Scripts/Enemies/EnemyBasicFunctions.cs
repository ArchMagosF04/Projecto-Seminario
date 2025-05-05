using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBasicFunctions : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    //[SerializeField]private float hp;
    private float spd;
    private float atk;
    private int techniquePoints;
   
    void Start()
    {
        gameObject.GetComponent<HealthComponent>().AssignHealth(stats.Hp);
        spd = stats.Spd;
        atk = stats.Atk;
    }

    void Update()
    {
        
    }   


    protected abstract void BasicAttack();
    protected abstract void SpecialAttack();
    protected abstract void SecretTechnique();


    protected virtual void chooseAtack()
    {
        int temp = Random.Range(0, 100);

        if (techniquePoints >= stats.MaxTechniquePoints)
        {
            SecretTechnique();
        }
        else if (temp < stats.SpecialAttackWeight)
        {
            SpecialAttack();
        }
        else 
        { 
            BasicAttack();
        }
        
    }
        
}
