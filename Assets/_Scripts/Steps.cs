using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Steps : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource;
    [SerializeField] AudioResource steps;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocityX != 0 && rb.velocityY == 0)
        {
            audioSource.loop = true;
            audioSource.resource = steps;
            audioSource.enabled = true;            
        }
        else
        {
            audioSource.enabled = false;
            audioSource.loop = false;
            audioSource.resource = null;            
        }
    }
}
