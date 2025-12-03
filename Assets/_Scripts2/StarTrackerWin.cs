using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarTrackerWin : StarTracker
{

    // Start is called before the first frame update
    void Start()
    {
        string lvlName = GlobalManager.Instance.GetCurrentLevel();
        string variableName = lvlName + "Stars";

        AddStars(StarsStringDecoder.DecodeString(PlayerPrefs.GetString(variableName)));
    }

    
}
