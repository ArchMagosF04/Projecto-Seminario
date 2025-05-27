using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeatDetector : MonoBehaviour
{
    private bool isOnBeat = false;
    [SerializeField] private float gracePeriod;
    private float timer = 0;

    public event Action<bool> OnBeat;


    void Update()
    {
        if (isOnBeat)
        {
            if (timer < gracePeriod)
            {
                timer += Time.deltaTime;
            }
            else if (timer >= gracePeriod)
            {
                isOnBeat = false;
                timer = 0;
                OnBeat.Invoke(false);
            }
        }
    }

    public void ActivateBeatEffect()
    {
        isOnBeat=true;
        OnBeat.Invoke(true);
    }
}
