using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public Transform PlayerTransform {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnWinGame()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void OnLoseGame()
    {
        SceneManager.LoadScene("LoseScreen");
    }
}
