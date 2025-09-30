using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance { get; private set; }  
    private float creationTime;

    private int currentSelectedWeapon;
    public int selectedWeapon { get { return currentSelectedWeapon; } }

    private string currentLevel;

    private bool firstTimePlaying;

    private void Awake()
    {
        if (Instance == null)
        {
            // This is the first instance, so set it as the persistent one
            Instance = this;
            creationTime = Time.time;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        }
        else
        {
            // If this is a new instance, check if it's newer than the existing one
            if (Time.time > creationTime)
            {
                // This instance was created earlier, so we destroy the current one
                Destroy(gameObject);
            }
            else
            {
                // This is the newer instance, destroy the old one
                Destroy(Instance.gameObject);
                Instance = this;
                creationTime = Time.time;
                DontDestroyOnLoad(gameObject);
            }
        }

        if (PlayerPrefs.GetInt("FirstTime")== 0)
        {
            firstTimePlaying = true;
        }
    }

    private void Start()
    {
        if (firstTimePlaying)
        {
            PlayerPrefs.SetInt("CompletedLevels", 0);
            PlayerPrefs.SetInt("CompletedTutorial", 0);
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.Save();
            firstTimePlaying = false;
        }        
    }

    public void SetCurrentWeapon(int currentWeapon)
    {
        currentSelectedWeapon = currentWeapon;
    }

    public void SetCurrentLevel(string newScene)
    {
        currentLevel = newScene;
    }

    public string GetCurrentScene()
    {
        return currentLevel;
    }

    public void RecordCompletedLvl(int level)
    {
        PlayerPrefs.SetInt("CompletedLevels", level);
        PlayerPrefs.Save();
    }

    public void RecordTutorialComplete()
    {
        PlayerPrefs.SetInt("CompletedTutorial", 1);
        PlayerPrefs.Save();
    }
}
