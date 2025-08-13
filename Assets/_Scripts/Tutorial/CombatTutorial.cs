using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class CombatTutorial : MonoBehaviour
{
    [SerializeField] GameObject[] messages;
    [SerializeField] GameObject[] arrows;
    [SerializeField] GameObject centerShadePanel;
    [SerializeField] GameObject playerShadePanel;
    [SerializeField] GameObject enemyShadePanel;
    [SerializeField] GameObject metronome;
    [SerializeField] TextMeshProUGUI hitCounter;
    [SerializeField] Mana energyBar;
    private static int index =0;
    private int count = 0;
    private static event Action<int> OnIndexChange = delegate { };
    private GameObject player;
    private float timer = 0.5f;
    private float currentTimer;
    [SerializeField] BeatDetector beatDetector;
    
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
        OnIndexChange += ShowMetronome;
        OnIndexChange += ShowEnergy;       
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (index == 1)
        {
            if(currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
            }
            else if (Input.anyKey)
            {
                AdvanceIndex();
                currentTimer = 0;
            }                
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

        if(index == 3)
        {
            if (currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
            }
            else if (Input.anyKey)
            {

                AdvanceIndex();
                currentTimer = 0;                               
            }
        }

        if (index == 4)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (beatDetector.IsOnBeat())
                {
                    AdvanceIndex();
                }                
            }
        }

        if(index == 5)
        {
            if (currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
            }
            else if (Input.anyKey)
            {
                AdvanceIndex();
                currentTimer = 0;
            }
        }

        if(index == 6)
        {
            if (count < 3 && Input.GetKeyDown(KeyCode.Mouse0) && beatDetector.IsOnBeat())
            {
                count++;
                hitCounter.text = count.ToString();
            }
            else if(count < 3 && Input.GetKeyDown(KeyCode.Mouse0) && !beatDetector.IsOnBeat())
            {
                count = 0;
                hitCounter.text = count.ToString();
            }
            else if (count >= 3)
            {
                AdvanceIndex();
                count = 0;
            }

        }

        if(index == 7)
        {
            if (currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
            }
            else if (Input.anyKey)
            {
                AdvanceIndex();
                currentTimer = 0;
            }
        }

        if(index == 8)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AdvanceIndex();
            }
        }
        
    }

    private void ShowEnemyHUD(int index)
    {
        if (index == 1)
        {
            TogglePlayerControls();
            arrows[0].SetActive(true);
            centerShadePanel.SetActive(true);
            bool a = centerShadePanel.activeSelf;
            playerShadePanel.SetActive(true);
        }
        else if (index != 1)
        {
            centerShadePanel.SetActive(false);
            playerShadePanel.SetActive(false);
            arrows[0].SetActive(false);
            TogglePlayerControls();
        }
    }

    private void TogglePlayerControls()
    {
        player.GetComponent<PlayerInput>().enabled = !player.GetComponent<PlayerInput>().enabled;
    }

    private void ShowMetronome(int index)
    {
        if(index == 3)
        {
            centerShadePanel.SetActive(true);
            arrows[1].SetActive(true);
        }
        else if (index == 4)
        {
            centerShadePanel.SetActive(false);
            arrows[1].SetActive(false);
        }
    }

    private void ShowEnergy(int index)
    {
        if(index == 5 || index == 7)
        {
            if(index == 7)
            {
                energyBar.IncreaseMana(100);
            }
            centerShadePanel.SetActive(true);
            enemyShadePanel.SetActive(true);
            arrows[2].SetActive(true);
        }
        else if(index != 5 || index !=7)
        {
            centerShadePanel.SetActive(false);
            enemyShadePanel.SetActive(false);
            arrows[2].SetActive(false);
        }
    }
}
