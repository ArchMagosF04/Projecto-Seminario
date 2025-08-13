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
    private BeatManager manager;

    public event Action<bool> OnBeat;

    private void Start()
    {
        
    }

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
                OnBeat?.Invoke(false);
            }
        }
    }

    public void ActivateBeatEffect()
    {
        if (OnBeat != null)
        {
            isOnBeat = true;
            OnBeat?.Invoke(true);
        }        
    }

    public void SetGracePeriod(float time)
    {
        gracePeriod = time;
    }

    public bool IsOnBeat() {  return isOnBeat; }
}
