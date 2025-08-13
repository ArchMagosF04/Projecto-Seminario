using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhibitPlayerMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField] CombatTutorial tutorialScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody);
        rigidbody.bodyType = RigidbodyType2D.Static;
        player = rigidbody.gameObject;
        tutorialScript.AdvanceIndex();
    }

    public void DestroyTrigger()
    {
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Destroy(this.gameObject);
    }
}
