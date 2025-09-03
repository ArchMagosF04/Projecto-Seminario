using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideMesages : MonoBehaviour
{
    [SerializeField] GameObject hideMessage;
    [SerializeField] GameObject showMessage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hideMessage!=null) hideMessage.SetActive(false);
        if (showMessage != null) showMessage.SetActive(true);
    }
}
