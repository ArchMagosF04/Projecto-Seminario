using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeToRythm : MonoBehaviour
{
    [SerializeField] BeatDetector beatDetector1;
    [SerializeField] BeatDetector beatDetector2;
    [SerializeField] BeatDetector beatDetector3;
    private bool shakeUp;
    private bool shakeDown;
    private Vector2 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        beatDetector1.OnBeat += BeatEffect;
        //beatDetector2.OnBeat += BeatEffect2;
        //beatDetector3.OnBeat += BeatEffect3;

    }  

    // Update is called once per frame
    void Update()
    {
       
    }

    private void BeatEffect(bool state)
    {
        shakeUp = state;
        if (state == true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
        }
        else if (state == false)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.1f);
        }
        
    }    

}
