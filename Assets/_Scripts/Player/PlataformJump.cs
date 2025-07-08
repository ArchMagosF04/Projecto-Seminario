using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformJump : MonoBehaviour
{
    public float distance;
    public LayerMask plataformLayer;
    public float time;
    float currentTime;
    public GameObject player;
    public GameObject plataform;


    private void Update()
    {
       bool detected = Physics2D.Raycast(transform.position, transform.up, distance, plataformLayer);
        
        if (detected && currentTime == 0)
        {
            DeactivateCollision();            
            currentTime += 0.1f;

        }

        if(currentTime > 0 && currentTime < time)
        {
            currentTime += Time.deltaTime;
        }
        else if(currentTime >= time)
        {
            ActivateCollision();
            currentTime = 0;
        }
    }

    private void DeactivateCollision()
    {
        player.GetComponent<Rigidbody2D>().excludeLayers += plataformLayer;
    }

    private void ActivateCollision()
    {
        player.GetComponent<Rigidbody2D>().includeLayers += plataformLayer;       

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plataform")
        {
            collision.gameObject.GetComponent<MovingPlataform>().AdoptPassangers();
        }
    }
}
