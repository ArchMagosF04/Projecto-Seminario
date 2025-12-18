using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneTutorial : MonoBehaviour
{
    [SerializeField] AutoDialogueManager autoDialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        if (autoDialogueManager.SelectedWeapon == 0)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            {
                this.gameObject.SetActive(false);
            }
        }
    }    
}
