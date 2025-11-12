using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] GameObject argentinaSprite;
    [SerializeField] GameObject brazilSprite;
    [SerializeField] GameObject accordeonLock;
    [SerializeField] GameObject saxofonLock;

    [SerializeField] LevelSelectorController levelSelectorController;

    void Start()
    {
        RefreshProgress();
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

        if (levelSelectorController.GetSelectedLevelIdex() == 0)
        {
            accordeonLock.SetActive(true);
        }

        if (PlayerPrefs.GetInt("CompletedTutorial") == 1)
        {
            argentinaSprite.SetActive(true);
        }
        else
        {
            argentinaSprite.SetActive(false);
        }

        switch (PlayerPrefs.GetInt("CompletedLevels"))
        {
            case 1:
            case 2:
                brazilSprite.SetActive(true);
                break;
            default:
                brazilSprite.SetActive(false);
                break;
        }
    }
}
