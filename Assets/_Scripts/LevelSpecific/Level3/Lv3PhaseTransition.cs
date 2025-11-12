using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3PhaseTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.OnWinGame();
    }
}
