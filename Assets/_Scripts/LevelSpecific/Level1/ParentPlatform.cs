using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlatform : MonoBehaviour
{
    private List<GameObject> objectsOnPlatform = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null) collision.transform.SetParent(transform);

        if (!objectsOnPlatform.Contains(collision.gameObject)) objectsOnPlatform.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != null) collision.transform.SetParent(null);
        if (objectsOnPlatform.Contains(collision.gameObject)) objectsOnPlatform.Remove(collision.gameObject);
    }

    private void OnDisable()
    {
        //foreach (GameObject obj in objectsOnPlatform)
        //{
        //    if (obj != null) objectsOnPlatform.Remove(obj);
        //}
    }
}
