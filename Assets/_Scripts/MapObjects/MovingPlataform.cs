using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    [SerializeField] Transform[] checkpoints;
    [SerializeField] float speed = 1;
    [SerializeField] BoxCollider2D trigger;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((checkpoints[index].position - transform.position).magnitude > 2)
        {
            transform.position += (checkpoints[index].position - transform.position).normalized * Time.deltaTime * speed;
            
        }
        else
        {
            if (index + 1 < checkpoints.Length)
            {
                index++;
            }
            else
            {
                index = 0;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Projectile")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            collision.transform.parent = null;
        }
    }
}
