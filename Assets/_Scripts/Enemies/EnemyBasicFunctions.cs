using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBasicFunctions : MonoBehaviour
{
    [SerializeField] private protected EnemyStats enemyInfo;
    private protected GameObject player;
    //[SerializeField]private float hp;
    private protected float spd;
    private protected float atk;
    private protected int techniquePoints;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        gameObject.GetComponent<HealthComponent>().AssignHealth(enemyInfo.Hp);
        spd = enemyInfo.MoveSpd;
        atk = enemyInfo.Atk;
        
    }

    void Update()
    {
        
    }   


    protected abstract void BasicAttack();
    protected abstract void SpecialAttack();
    protected abstract void SecretTechnique();


    protected virtual void chooseAtack()
    {
        //int temp = Random.Range(0, 100);

        //if (techniquePoints >= enemyInfo.MaxTechniquePoints)
        //{
        //    SecretTechnique();
        //}
        //else if (temp < enemyInfo.SpecialAttackWeight)
        //{
        //    SpecialAttack();
        //}
        //else 
        //{ 
        //    BasicAttack();
        //}
        
    }

    private protected float GetPlayerRalativeDirection()
    {
        Vector3 direction = player.transform.position - transform.position;
        return Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
    }
        
}
