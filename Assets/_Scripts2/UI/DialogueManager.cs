using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject[] IntroDialogue;
    private float waitTime = 3;
    private float currentWaitTime = 0;

    [SerializeField] private GameObject[] MidCombatDialogue;

    [SerializeField] private float screenTime;
    public bool showMessagesAtRandom = false;
    private float messageInterval = 5f;
    private float currentMessageTimer = 0;

    private bool introEnded = false;
    private bool messageOnScreen = false;

    private int mainIndex = 0;
    private int subIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<ISpeaker>().StartSpeaking();
        enemy.GetComponent<ISpeaker>().StartSpeaking();

    }

    // Update is called once per frame
    void Update()
    {
        if (introEnded == false)
        {
            if(mainIndex == 0)
            {
                IntroDialogue[mainIndex].gameObject.SetActive(true);
            }

            if (currentWaitTime < waitTime)
            {
                currentWaitTime += Time.deltaTime;                
            }
            else if (currentWaitTime >= waitTime)
            {
                if (Input.anyKey)
                {
                    currentWaitTime = 0;
                    AdvanceIndex();

                }
            }
        }
        else
        {
            float enemyHp = enemy.GetComponent<ISpeaker>().GetHealth();

            if (!showMessagesAtRandom)
            {
                if (enemyHp / 100 < 0.25)
                {
                    StartCoroutine(ShowMessage(2));
                }
                else if (enemyHp / 100 < 0.50)
                {
                    StartCoroutine(ShowMessage(1));
                }
                else if (enemyHp / 100 < 0.75)
                {
                    StartCoroutine(ShowMessage(0));
                }
            }
            else if (showMessagesAtRandom)
            {
                if(currentMessageTimer < messageInterval)
                {
                    currentMessageTimer += Time.deltaTime;
                }
                else if(currentMessageTimer >= messageInterval)
                {
                    StartCoroutine(ShowMessage(ChoseDialogue()));
                    currentMessageTimer = 0;
                }
                    
            }
            

        }
    }

    public void AdvanceIndex()
    {
        if (mainIndex < IntroDialogue.Length - 1)
        {
            IntroDialogue[mainIndex].gameObject.SetActive(false);
            mainIndex++;
            IntroDialogue[mainIndex].gameObject.SetActive(true);
        }
        else
        {
            IntroDialogue[mainIndex].gameObject.SetActive(false);
            introEnded = true;
            player.GetComponent<ISpeaker>().StopSpeaking();
            enemy.GetComponent<ISpeaker>().StopSpeaking();
        }
        
    }

    private int ChoseDialogue()
    {
       return subIndex = Random.Range(0, MidCombatDialogue.Length);
    }

    //public void ShowMessage(int index)
    //{
    //    MidCombatDialogue[index].gameObject.SetActive(true);
    //}

    IEnumerator ShowMessage(int index)
    {
        if (!showMessagesAtRandom && MidCombatDialogue[index] != null)
        {
            MidCombatDialogue[index].gameObject.SetActive(true);

            yield return new WaitForSeconds(screenTime);

            MidCombatDialogue[index].gameObject.SetActive(false);
            Destroy(MidCombatDialogue[index]);
        }
        else
        {
            MidCombatDialogue[index].gameObject.SetActive(true);

            yield return new WaitForSeconds(screenTime);

            MidCombatDialogue[index].gameObject.SetActive(false);
        }

            yield return null;

        StopCoroutine(ShowMessage(index));
    }


}
