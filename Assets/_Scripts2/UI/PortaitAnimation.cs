using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaitAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Core_Mana.ManaIsFull += PlayAnimation;
    }

    private void PlayAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger("play");
    }

    private void OnDisable()
    {
        Core_Mana.ManaIsFull -= PlayAnimation;
    }
}
