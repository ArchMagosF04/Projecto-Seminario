using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public PlayerController PlayerInstance {  get; private set; }

    [field: SerializeField] public bool IsGameActive { get; private set; }

    [SerializeField] private PlayerInput playerInput;

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

        playerInput = GameInputManager.Instance.gameObject.GetComponent<PlayerInput>();
    }

    public void ToggleGameActiveState(bool state) => IsGameActive = state;

    public void OnGameWon()
    {
        Debug.Log("GAME WON");
        GameEnd();
        winScreen.OpenMenu();
    }

    public void OnGameLost()
    {
        Debug.Log("GAME LOST");
        GameEnd();
        loseScreen.OpenMenu();
    }

    private void GameEnd()
    {
        IsGameActive = false;
        BeatManager.Instance.ToggleMusic(false);

        Time.timeScale = 1;
    }

    public void MakePlayerInvincible()
    {
        PlayerInstance.Health.ToggleInvincibility(true);
        playerInput.SwitchCurrentActionMap("UI");
    }    
}
