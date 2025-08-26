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
    [SerializeField] GameObject beatManager;
    [SerializeField] GameObject enemyHud;
    [SerializeField] TextMeshProUGUI hitCounter;
    [SerializeField] Core_Mana energyBar;
    private static int index =0;
    public static int Index {  get { return index; } }
    private int count = 0;
    private static event Action<int> OnIndexChange = delegate { };
    private GameObject player;
    private float timer = 0.5f;
    private float currentTimer;
    [SerializeField] BeatDetector beatDetector;
    private bool keepGoing = false;
    
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

    private void FixedUpdate()
    {
        if (index == 4)
        {
            if(currentTimer < timer)
            {
                currentTimer += 0.5f;
            } 
            
        }
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
                currentTimer = 0;
                AdvanceIndex();                
            }
        }    
        
        if (index == 4)
        {
            if (beatDetector.IsOnBeat())
            {                
                StartCoroutine(StopGame());
                Time.timeScale = 0;
            }

            if (keepGoing)
            {
                StopCoroutine(StopGame());
                AdvanceIndex();
                
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
                count = 0;
            }
        }

        if(index == 6)
        {           
            if (count >= 3)
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
                TogglePlayerControls();
                AdvanceIndex();
                
            }
        }       
        
    }

    private void ShowEnemyHUD(int index)
    {
        if (index == 1)
        {
            TogglePlayerControls();
            enemyHud.SetActive(true);
            arrows[0].SetActive(true);
            centerShadePanel.SetActive(true);            
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

    public void TogglePlayerControls()
    {
        //player.GetComponent<PlayerController>().enabled = !player.GetComponent<PlayerController>().enabled;
    }

    private void ShowMetronome(int index)
    {
        if(index == 3)
        {
            metronome.SetActive(true);
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

    IEnumerator StopGame()
    {
        //if (beatDetector.IsOnBeat())
        //{
        //    //AdvanceIndex();
        //    Time.timeScale = 0;
        //}        

        yield return new WaitForSecondsRealtime(2);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));        

        Time.timeScale = 1;   

        keepGoing = true;    

        yield break;        
    }

    public void RiseCount()
    {
        count++;
        hitCounter.text = count.ToString();
    }

    public void ResetCount()
    {
        count = 0;
        hitCounter.text = count.ToString();
    }

    public int GetIndex()
    {
        return index;
    }
    
}
