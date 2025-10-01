using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] GameObject argentinaSprite;
    [SerializeField] GameObject brazilSprite;
    [SerializeField] GameObject accordeonLock;

    void Start()
    {
        RefreshProgress();
    }

    public void RefreshProgress()
    {
        if (PlayerPrefs.GetInt("AccordeonUnlocked") == 1)
        {
            accordeonLock.SetActive(false);
            brazilSprite.SetActive(true);
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
