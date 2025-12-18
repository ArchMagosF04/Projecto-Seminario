using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalResultsController : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject[] endingTiers;

    [SerializeField, TextArea] private string[] finalTextLines;

    [SerializeField] private TMP_Text finalText;

    private int starsUnloked;

    private void Awake()
    {
        foreach (GameObject slider in endingTiers) slider.SetActive(false);
    }

    private void Start()
    {
        if (starsUnloked >= 10)
        {
            endingTiers[4].SetActive(true);
            finalText.text = finalTextLines[4];
        }
        else if (starsUnloked >= 8)
        {
            endingTiers[3].SetActive(true);
            finalText.text = finalTextLines[3];
        }
        else if (starsUnloked >= 6)
        {
            endingTiers[2].SetActive(true);
            finalText.text = finalTextLines[2];
        }
        else if (starsUnloked >= 3)
        {
            endingTiers[1].SetActive(true);
            finalText.text = finalTextLines[1];
        }
        else
        {
            endingTiers[0].SetActive(true);
            finalText.text = finalTextLines[0];
        }
    }

    public void LoadData(GameData gameData)
    {
        starsUnloked = gameData.GetAmountOfUnlockedStars();
    }

    public void SaveData(GameData gameData)
    {
        
    }
}
