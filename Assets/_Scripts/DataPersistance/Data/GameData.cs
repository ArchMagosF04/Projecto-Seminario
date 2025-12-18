using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public bool unlocked1stLevel;
    public bool unlocked2ndLevel;
    public bool unlocked3rdLevel;

    public bool unlockedweapon2;
    public bool unlockedweapon3;
    public bool unlockedweapon4;

    public SerializableDictionary<string, bool> starsGained;

    public int weaponSelected;
    public int lastLevelSelected;

    //Values defined in the constructor will be the default values the game starts when there is no data to load.
    public GameData()
    {
        this.unlocked1stLevel = false;
        this.unlocked2ndLevel = false;
        this.unlocked3rdLevel = false;

        this.unlockedweapon2 = false;
        this.unlockedweapon3 = false;
        this.unlockedweapon4 = false;

        starsGained = new SerializableDictionary<string, bool>();

        InitializeDictionary();

        weaponSelected = 0;
        lastLevelSelected = 0;
    }

    public int GetPercentageComplete()
    {
        int totalCompleted = 0;

        if (unlocked1stLevel) totalCompleted++;
        if (unlocked2ndLevel) totalCompleted++;
        if (unlocked3rdLevel) totalCompleted++;

        if (unlockedweapon2) totalCompleted++;
        if (unlockedweapon3) totalCompleted++;
        if (unlockedweapon4) totalCompleted++;

        foreach(bool starUnlocked in starsGained.Values)
        {
            if (starUnlocked) totalCompleted++;
        }

        int percentageCompleted = -1;
        if (starsGained.Count + 6 != 0)
        {
            percentageCompleted = (totalCompleted * 100 / (starsGained.Count + 6));
        }

        return percentageCompleted;
    }

    public int GetAmountOfUnlockedStars()
    {
        int amount = 0;

        foreach(bool starUnlocked in starsGained.Values)
        {
            if (starUnlocked) amount++;
        }

        return amount;
    }

    private void InitializeDictionary()
    {
        starsGained.Add("Star-Tutorial", false);
        starsGained.Add("Star-1A", false);
        starsGained.Add("Star-1B", false);
        starsGained.Add("Star-1C", false);
        starsGained.Add("Star-2A", false);
        starsGained.Add("Star-2B", false);
        starsGained.Add("Star-2C", false);
        starsGained.Add("Star-3A", false);
        starsGained.Add("Star-3B", false);
        starsGained.Add("Star-3C", false);
    }
}
