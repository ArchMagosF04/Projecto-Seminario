using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeToSides : MonoBehaviour
{
    public bool randomValue = false;

    public void Shake(bool activate, BeatDetector beatDetector)
    {
        if (activate)
        {
            beatDetector.OnBeat += BeatEffect;
            
        }
        else if (activate == false)
        {
            beatDetector.OnBeat -= BeatEffect;           
        }

    }

    private void BeatEffect(bool state)
    {
        float r = Random.Range(0.5f,1);
        if (state == true)
        {
            transform.position = new Vector2(transform.position.x + r, transform.position.y);
        }
        else if (state == false)
        {
            transform.position = new Vector2(transform.position.x - r, transform.position.y);
        }
    }
}
