using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Transform[] checkpoints;
    [SerializeField] float speed = 1;  
    int index = 0;
    List<GameObject> passangers = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        foreach (GameObject passanger in passangers)
        {
            passanger.GetComponent<Rigidbody2D>().velocity += rb.velocity;
        }
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

        foreach (GameObject passanger in passangers)
        {
            if (Mathf.Abs(passanger.GetComponent<Rigidbody2D>().velocity.x) > 1)
            {
                passanger.transform.parent = null;
            }
            else
            {
                passanger.transform.parent = transform;
            }
        }
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            collision.transform.parent = transform;
            passangers.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            collision.transform.parent = null;
            passangers.Remove(collision.gameObject);
        }
    }    
}
