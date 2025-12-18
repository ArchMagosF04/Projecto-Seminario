using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStars : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject unlockedStar;

    private bool hasPreviouslyCompletedTheLevel;
    private bool hasCompletedTheLevel;

    private void Awake()
    {
        unlockedStar.SetActive(false);
        hasCompletedTheLevel = false;
    }

    public void CalculateResults()
    {
        hasCompletedTheLevel = true;
    }

    public void ShowStars()
    {
        unlockedStar.SetActive(true);
    }

    public void LoadData(GameData gameData)
    {
        hasPreviouslyCompletedTheLevel = gameData.starsGained["Star-Tutorial"];
    }

    public void SaveData(GameData gameData)
    {
        if (hasPreviouslyCompletedTheLevel || hasCompletedTheLevel)
        {
            gameData.starsGained["Star-Tutorial"] = true;
        }

        if (hasCompletedTheLevel)
        {
            gameData.unlocked1stLevel = true;
            MenuBackground.backgroundIndex = 1;
            PlayerPrefs.SetInt("background", 1);
        }
    }
}
