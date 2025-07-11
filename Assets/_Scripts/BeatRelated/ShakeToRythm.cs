using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeToRythm : MonoBehaviour
{
    [SerializeField] BeatDetector beatDetector1;
    [SerializeField] bool suscribeOnStart;
    //[SerializeField] BeatDetector beatDetector2;
    //[SerializeField] BeatDetector beatDetector3;
    
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        if (suscribeOnStart)
        {
            beatDetector1.OnBeat += BeatEffect;
        }
        
        //beatDetector2.OnBeat += BeatEffect2;
        //beatDetector3.OnBeat += BeatEffect3;

    }  

    // Update is called once per frame
    void Update()
    {
       
    }

    private void BeatEffect(bool state)
    {        
        if (state == true)
        {
            transform.position = new Vector3(transform.position.x +0.3f, transform.position.y + 0.3f, transform.position.z);
        }
        else if (state == false)
        {
            transform.position = initialPos;
        }
        
    }  
    
    public void ActivateShake(bool status, BeatDetector beatDetector)
    {
        if (status == true)
        {
            beatDetector.OnBeat += BeatEffect;
        }
        if (status == false)
        {
            beatDetector.OnBeat -= BeatEffect;
        }
        
    }

}
