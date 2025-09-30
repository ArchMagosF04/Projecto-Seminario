using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTooltip : MonoBehaviour
{
    private GameObject message;

    // Start is called before the first frame update
    void Start()
    {
        message.SetActive(false);        
    }   

    public void ShowTooltip()
    {
        message.SetActive(true);
    }

    public void HideTooltip()
    {
        message.SetActive(false);
    }

}
