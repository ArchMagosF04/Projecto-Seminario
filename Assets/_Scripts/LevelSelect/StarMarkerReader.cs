using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarMarkerReader : MonoBehaviour, IDataPersistance
{
    [SerializeField] private MarkerStarItem[] markerStarItems;

    private void Awake()
    {
        foreach (MarkerStarItem item in markerStarItems)
        {
            item.StarObject.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        foreach (MarkerStarItem item in markerStarItems)
        {
            if (item.IsStarUnlocked) item.StarObject.gameObject.SetActive(true);
        }
    }

    public void LoadData(GameData gameData)
    {
        for (int i = 0; i < markerStarItems.Length; i++)
        {
            string starKey = "Star-" + markerStarItems[i].StarID;

            if (gameData.starsGained.ContainsKey(starKey))
            {
                markerStarItems[i].IsStarUnlocked = gameData.starsGained[starKey];
            }
            else
            {
                Debug.LogError("Wrong Star Key", this);
            }
        }
    }

    public void SaveData(GameData gameData)
    {
        
    }

    [Serializable]
    private struct MarkerStarItem
    {
        public string StarID;
        public Image StarObject;
        public bool IsStarUnlocked;
    }
}
