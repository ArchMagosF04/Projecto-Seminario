using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStarsTracker : MonoBehaviour, IDataPersistance
{
    public static LevelStarsTracker Instance;

    [Header("Time Requirements")]
    [SerializeField] private TMP_Text timerText;

    private float elapseTime;

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

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            elapseTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapseTime / 60);
            int seconds = Mathf.FloorToInt(elapseTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(GameData gameData)
    {
        
    }
}
