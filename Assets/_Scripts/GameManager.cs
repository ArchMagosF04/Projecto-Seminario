using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject pauseScreen;

    public bool IsGamePaused { get; private set; }

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

        BeatManager.Instance.ToggleMusic(true);
        pauseScreen.SetActive(false);
    }

    public void PauseMenu(bool input)
    {
        IsGamePaused = input;
        if (input)
        {
            pauseScreen.SetActive(true);
            BeatManager.Instance.ToggleMusic(false);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            BeatManager.Instance.ToggleMusic(true);
        }
    }
}
