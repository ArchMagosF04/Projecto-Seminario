using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhibitPlayerMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField] CombatTutorial tutorialScript;
    public bool activate = false;

    private void Update()
    {
        if (activate)
        {
            player.GetComponent<Rigidbody2D>().velocityX = 0; 
            player.GetComponent<Rigidbody2D>().velocityY = 0;
        }
        if(CombatTutorial.Index == 9)
        {
            player.gameObject.GetComponent<ISpeaker>().TurnOn();
            DestroyTrigger();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody);
        //rigidbody.bodyType = RigidbodyType2D.Static;
        player = rigidbody.gameObject;
        player.gameObject.GetComponent<ISpeaker>().TurnOff();
        tutorialScript.AdvanceIndex();        
        activate = true;
    }

    public void DestroyTrigger()
    {
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Destroy(this.gameObject);
    }
}
