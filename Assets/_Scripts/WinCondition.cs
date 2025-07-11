using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class WinCondition : MonoBehaviour
{   

    [SerializeField] HealthComponent playerHealth;
    [SerializeField] HealthComponent enemyHealth;

    private void Start()
    {
        playerHealth.OnDeath += LoseScreen;
        enemyHealth.OnDeath += WinScreen;
    }


    public static void WinScreen()
    {
        SceneManager.LoadScene(3);
    }

    public static void LoseScreen()
    {
        SceneManager.LoadScene(4);
    }

    
}
