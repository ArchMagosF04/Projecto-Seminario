using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockNextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalManager.Instance.RecordCompletedLvl(GlobalManager.Instance.GetCurentLevelIndex());
    }    
}
