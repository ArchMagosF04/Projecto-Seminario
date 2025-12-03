using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] GameObject argentinaSprite;
    [SerializeField] GameObject brazilSprite;
    [SerializeField] GameObject mexicoSprite;
    [SerializeField] GameObject accordeonLock;
    [SerializeField] GameObject saxofonLock;

    [SerializeField] LevelSelectorController levelSelectorController;

    [SerializeField] private StarTracker[] starTrackerList;

    void Start()
    {
        argentinaSprite.SetActive(false);
        brazilSprite.SetActive(false);
        mexicoSprite.SetActive(false);
        RefreshProgress();
    }

    private void FixedUpdate()
    {
        if (levelSelectorController.GetSelectedLevelIdex() == 0)
        {
            accordeonLock.SetActive(true);
            saxofonLock.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.GetInt("AccordeonUnlocked") == 1) accordeonLock.SetActive(false);
            if (PlayerPrefs.GetInt("SaxofonUnlocked") == 1) saxofonLock.SetActive(false);
        }

        RefreshProgress();
    }



    public void RefreshProgress()
    {
        showEmptyStars(0);

        if (PlayerPrefs.GetInt("AccordeonUnlocked") == 1 && levelSelectorController.GetSelectedLevelIdex() != 0)
        {
            accordeonLock.SetActive(false);
        }

        if (PlayerPrefs.GetInt("SaxofonUnlocked") == 1 && levelSelectorController.GetSelectedLevelIdex() != 0)
        {
            saxofonLock.SetActive(false);
        }       

        if (PlayerPrefs.GetInt("CompletedTutorial") == 1)
        {
            argentinaSprite.SetActive(true);
            bool[] temp = new bool[1];
            temp[0] = true;
            AddStars(0, temp);
            showEmptyStars(1);
        }
        else
        {
            argentinaSprite.SetActive(false);
        }

        if (PlayerPrefs.GetInt("CompletedLevels") >= 1)
        {
            brazilSprite.SetActive(true);
            showEmptyStars(2);
            PlayerPrefs.SetInt("AccordeonUnlocked", 1);
            
            AddStars(1, GetStarAmount(PlayerPrefs.GetString("Level1V2Stars")));
        }

        if (PlayerPrefs.GetInt("CompletedLevels") >= 2)
        {
            mexicoSprite.SetActive(true);
            showEmptyStars(3);
            PlayerPrefs.SetInt("SaxofonUnlocked", 1);
            AddStars(2, GetStarAmount(PlayerPrefs.GetString("Level2Stars")));
        }

        if (PlayerPrefs.GetInt("CompletedLevels") >= 3)
        {            
            AddStars(3, GetStarAmount(PlayerPrefs.GetString("Level3Stars")));
        }




    }

    public void UnlockAll()
    {
        PlayerPrefs.SetInt("CompletedTutorial", 1);
        argentinaSprite.SetActive(true);

        brazilSprite.SetActive(true);
        PlayerPrefs.SetInt("AccordeonUnlocked", 1);

        mexicoSprite.SetActive(true);
        PlayerPrefs.SetInt("SaxofonUnlocked", 1);
    }

    private void AddStars(int level, bool[] flags)
    {
        starTrackerList[level].AddStars(flags);
    }

    private void showEmptyStars(int level)
    {
        starTrackerList[level].showEmptyStars = true;
    }

    private bool[] GetStarAmount(string value)
    {
       bool[] temp = StarsStringDecoder.DecodeString(value);
       int result = 0;

       foreach(bool flag in temp)
       {
         if (flag) result++;
       }

       return temp;

    }
}
