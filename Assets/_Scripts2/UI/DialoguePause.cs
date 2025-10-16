using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePause : MonoBehaviour
{
    [SerializeField] DialogueManager Dmanager;
    [SerializeField] PlayerController player;
    private void OnDisable()
    {
        //player.StopSpeaking();
        //Dmanager.enabled = false;
        Dmanager.Pause();
    }

    private void OnDestroy()
    {
        player.StopSpeaking();
        Dmanager.enabled = false;
    }
}
