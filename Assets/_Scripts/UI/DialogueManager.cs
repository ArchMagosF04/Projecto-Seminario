using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private PlayerInput player;

    [SerializeField] private GameObject[] IntroDialogue;
    private float waitTime = 3;
    private float currentWaitTime = 0;

    [SerializeField] private GameObject[] MidCombatDialogue;
    [SerializeField] private float screenTime;

    private bool introEnded = false;
    private bool messageOnScreen = false;

    private int mainIndex = 0;
    private int subIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player.enabled = false;
        enemy.GetComponent<ISpeaker>().TurnOff();
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
            player.enabled = true;
            enemy.GetComponent<ISpeaker>().TurnOn();
        }
        
    }

    private void ChoseDialogue()
    {
       subIndex = Random.Range(0, MidCombatDialogue.Length);
    }

    public void ShowMessage(int index)
    {
        MidCombatDialogue[index].gameObject.SetActive(true);
    }


}
