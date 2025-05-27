using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeatTester : MonoBehaviour
{
    private bool isOnBeat = false;
    [SerializeField] private float gracePeriod;
    private float timer = 0;

    public GameObject testObject;

    // Start is called before the first frame update
    void Start()
    {
        //BeatManager.Instance.OnBeat += ActivateBeatEffect;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnBeat)
        {
            //print("OnBeat");
            testObject.SetActive(true);
            //isOnBeat = false;

            if (timer < gracePeriod)
            {
                timer += Time.deltaTime;
            }
            else if (timer >= gracePeriod)
            {                
                isOnBeat = false;
                timer = 0;
                testObject.SetActive(false);
            }
        }        
    }

    public void ActivateBeatEffect()
    {
            isOnBeat = true;
    }
}
