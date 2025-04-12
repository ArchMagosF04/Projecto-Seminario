using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeatTester : MonoBehaviour
{
    private bool isOnBeat = false;
    [SerializeField] private float gracePeriod;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        BeatManager.Instance.OnBeat += ActivateBeatEffect;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnBeat)
        {
            if (timer < gracePeriod) 
            {
                timer += Time.deltaTime;
            }
            else if (timer > gracePeriod)
            {
                timer = 0;
                isOnBeat = false;
            }

            print("OnBeat");
        }
    }

    private void ActivateBeatEffect()
    {
        isOnBeat = true;
    }
}
