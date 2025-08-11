using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorial : MonoBehaviour
{
    [SerializeField] GameObject[] messages;
    [SerializeField] GameObject[] arrows;
    [SerializeField] GameObject mainShadePanel;
    [SerializeField] GameObject playerShadePanel;
    [SerializeField] GameObject enemyShadePanel;
    private int index;
    private int count = 0;
    private event Action<int> OnIndexChange = delegate { };
    
    public void AdvanceIndex()
    {
        index++;
        messages[index].SetActive(true);
        messages[index-1].SetActive(false);
        OnIndexChange(index);
    }

    private void Start()
    {
        OnIndexChange += ShowEnemyHUD;
    }

    private void Update()
    {
        if(index == 1 && Input.anyKey)
        {
            AdvanceIndex();
        }

        if (index == 2 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(count < 3)
            {
                count++;
            }
            else
            {
                AdvanceIndex();
                count = 0;
            }                
        }
    }

    private void ShowEnemyHUD(int index)
    {
        if (index == 1)
        {
            arrows[0].SetActive(true);
            mainShadePanel.SetActive(true);
            playerShadePanel.SetActive(true);
        }
        else if (index == 2)
        {
            mainShadePanel.SetActive(false);
            playerShadePanel.SetActive(false);
            arrows[0].SetActive(false);
        }
    }
}
