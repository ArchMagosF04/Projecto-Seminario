using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ProjectileEnimationEvents : MonoBehaviour
{
    private Animator anim;
    private int charge;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
             
        gameObject.GetComponent<Projectile>().destroy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Hit");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetCharge(int value)
    {
      anim.SetInteger("ChargeLevel", value);
    }
}
