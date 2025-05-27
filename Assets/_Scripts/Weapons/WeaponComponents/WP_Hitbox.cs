using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_Hitbox : MonoBehaviour
{
    public List<Collider2D> collider2Ds { get; private set; }

    private void Start()
    {
        collider2Ds = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter2D");
        collider2Ds.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerExit2D");
        collider2Ds.Remove(collision);
    }
}
