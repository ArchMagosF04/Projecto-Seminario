using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Transform[] checkpoints;
    [SerializeField] float speed = 1;
    public float Speed { get { return speed; } }
    int index = 0;
    List<GameObject> passangers = new List<GameObject>();

    private bool shake;
    private Vector2 initialPos;

    private bool movingRight;
    public bool MovingRight { get { return movingRight; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        foreach (GameObject passanger in passangers)
        {
            if (passanger != null && passanger.GetComponent<Rigidbody2D>() != null)
            {
                passanger.GetComponent<Rigidbody2D>().velocity += rb.velocity;
            }            
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

        if (!shake)
        {
            foreach (GameObject passanger in passangers)
            {
                if (Mathf.Abs(passanger.GetComponent<Rigidbody2D>().velocity.x) > 0.7)
                {
                    passanger.transform.parent = null;
                }
                else
                {
                    passanger.transform.parent = transform;
                }
            }
        }
        

        GetMovingDirection((checkpoints[index].position - transform.position).normalized);

        
    }

    private void GetMovingDirection(Vector3 NormalizedDirection)
    {
       float temp = Vector3.Cross(NormalizedDirection, Vector3.right).y;
        if(temp > 0)
        {
            movingRight = true;
        }
        else if(temp < 0)
        {
            movingRight=false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile" && !shake)
        {
            collision.transform.parent = transform;
            passangers.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile" && passangers != null && passangers.Count != 0)
        {
            collision.transform.parent = null;
            passangers.Remove(collision.gameObject);
        }
    }
    
    public void Shake(bool activate, BeatDetector beatDetector)
    {
        if (activate)
        {
            beatDetector.OnBeat += BeatEffect;
            shake = true;
            AbandonPassangers();
        }
        else if (activate == false)
        {
            beatDetector.OnBeat -= BeatEffect;
            shake = false;
            AdoptPassangers();
        }

    }

    private void BeatEffect(bool state)
    {
        if (state == true)
        {
            transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y);
        }
        else if (state == false)
        {
            transform.position = new Vector2(transform.position.x - 0.3f, transform.position.y);
        }
    }

    private void AbandonPassangers()
    {
        if (passangers != null && passangers.Count != 0)
        {
            foreach (var passanger in passangers)
            {
                passanger.transform.parent = null;
            }
        }               
    }

    private void AdoptPassangers()
    {
        if(passangers != null && passangers.Count != 0)
        {
            foreach (var passanger in passangers)
            {
                passanger.transform.parent = transform;
            }
        }        
    }
}
