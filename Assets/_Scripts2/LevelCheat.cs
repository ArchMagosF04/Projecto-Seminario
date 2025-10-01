using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheat : MonoBehaviour
{
    public string code;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            code += "s";
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            code += "t";
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            code += "a";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            code += "r";
        }

        if(code == "star")
        {
            PlayerPrefs.SetInt("CompletedLevels", 2);
            PlayerPrefs.SetInt("CompletedTutorial", 1);
            PlayerPrefs.SetInt("AccordeonUnlocked", 1);
            Destroy(gameObject);
        }
    }
}
