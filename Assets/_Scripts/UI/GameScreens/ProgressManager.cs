using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] GameObject argentinaSprite;
    [SerializeField] GameObject brazilSprite;
    [SerializeField] GameObject mexicoSprite;
    [SerializeField] GameObject accordeonLock;
    [SerializeField] GameObject saxofonLock;

    [SerializeField] LevelSelectorController levelSelectorController;

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
    }



    public void RefreshProgress()
    {
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
        }
        else
        {
            argentinaSprite.SetActive(false);
        }

        if (PlayerPrefs.GetInt("CompletedLevels") >= 1)
        {
            brazilSprite.SetActive(true);
            PlayerPrefs.SetInt("AccordeonUnlocked", 1);
        }

        if (PlayerPrefs.GetInt("CompletedLevels") >= 2)
        {
            mexicoSprite.SetActive(true);
            PlayerPrefs.SetInt("SaxofonUnlocked", 1);
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
}
