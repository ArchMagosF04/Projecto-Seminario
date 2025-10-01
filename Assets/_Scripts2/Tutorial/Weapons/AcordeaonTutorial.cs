using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcordeaonTutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    
    // Start is called before the first frame update
    void Start()
    {
        tutorial.SetActive(false);

        if (PlayerPrefs.GetInt("AcordeonTutorial") == 1)
        {
            Destroy(tutorial);
            Destroy(this);
        }
        else
        {
            GameObject.Find("DialogueManager").GetComponent<DialogueManager>().AddIntroMessage(tutorial);
            PlayerPrefs.SetInt("AcordeonTutorial", 1);
        }
    }

    
}
