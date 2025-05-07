using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cerati : EnemyBasicFunctions
{
    // Start is called before the first frame update
    private float time= 5;
    private float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < time)
        {
            timer += Time.deltaTime;
        }
        else if (timer > time) 
        { 
            timer = 0;
            BasicAttack();
        }
    }

    protected override void BasicAttack()
    {
        GameObject bullet = GameObject.Instantiate(enemyInfo.GetProyectile(0), transform.position, Quaternion.Euler(0,0, GetPlayerRalativeDirection()));
        bullet.GetComponent<ProyectileBase>().Initialize(atk,this.gameObject);
    }



    protected override void SecretTechnique()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpecialAttack()
    {
        throw new System.NotImplementedException();
    }


}
