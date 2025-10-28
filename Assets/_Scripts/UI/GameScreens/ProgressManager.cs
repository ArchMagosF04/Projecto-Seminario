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

    private void FixedUpdate()
    {
        RefreshProgress();
    }

    public void RefreshProgress()
    {
        if (PlayerPrefs.GetInt("AccordeonUnlocked") == 1 && levelSelectorController.GetSelectedLevelIdex()!=0)
        {
            accordeonLock.SetActive(false);
            brazilSprite.SetActive(true);
        }

        if (PlayerPrefs.GetInt("SaxofonUnlocked") == 1 && levelSelectorController.GetSelectedLevelIdex() != 0)
        {
            saxofonLock.SetActive(false);
            brazilSprite.SetActive(true);
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
    }
}
