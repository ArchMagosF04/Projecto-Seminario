using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public PlayerController PlayerInstance {  get; private set; }

    [field: SerializeField] public bool IsGameActive { get; private set; }

    [Header("Game End Screens")]
    [SerializeField] private MenuPage winScreen;
    [SerializeField] private MenuPage loseScreen;

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

    public void ToggleGameActiveState(bool state) => IsGameActive = state;

    public void OnGameWon()
    {
        Debug.Log("GAME WON");
        winScreen.OpenMenu();
    }

    public void OnGameLost()
    {
        Debug.Log("GAME LOST");
        loseScreen.OpenMenu();
    }
}
