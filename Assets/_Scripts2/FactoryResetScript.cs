using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class FactoryResetScript : MonoBehaviour
{
    public void FactoryReset()
    {
        PlayerPrefs.SetInt("CompletedLevels", 0);
        PlayerPrefs.SetInt("CompletedTutorial", 0);
        PlayerPrefs.SetInt("AccordeonUnlocked", 0);
        PlayerPrefs.SetInt("AcordeonTutorial", 0);
        PlayerPrefs.SetInt("SaxofonUnlocked", 0);
    }
}
