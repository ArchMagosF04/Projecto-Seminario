using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] BeatDetector beatDetector;
    private int hp = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp < 15)
        {
            gameObject.GetComponent<HealthComponent>().Heal(100);
            hp = 100;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 9)
    //    {
    //        GameObject weapon = collision.gameObject;
    //        gameObject.GetComponent<HealthComponent>().TakeDamage(15);
    //        print("Dummy taking damage");
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (beatDetector.IsOnBeat())
            {                
                gameObject.GetComponent<HealthComponent>().TakeDamage(5*3);
                hp -= 5*3;
                print("Dummy taking EXTRA damage");
            }

            gameObject.GetComponent<HealthComponent>().TakeDamage(5);
            hp -= 5;
            print("Dummy taking REGULAR damage");
        }


    }
}
