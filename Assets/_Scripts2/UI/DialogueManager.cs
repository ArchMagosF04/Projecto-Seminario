using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{ 
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;


    [SerializeField] GameObject panel;
    [SerializeField] private List<GameObject> IntroDialogue;
    [SerializeField]private float waitTime = 2;
    private float currentWaitTime = 0;
    [SerializeField] GameObject skipText;
    [SerializeField] GameObject skipText2;
    [SerializeField] private GameObject screenBorders;

    [SerializeField] private GameObject[] MidCombatDialogue;

    [SerializeField] private float screenTime;
    public bool showMessagesAtRandom = false;
    private float messageInterval = 5f;
    private float currentMessageTimer = 0;

    private bool introEnded = false;
    //private bool messageOnScreen = false;

    private int mainIndex = 0;
    private int subIndex = 0;

    // Start is called before the first frame update
    private void Awake()
    {       

    }

    void Start()
    {
        if (IntroDialogue != null && IntroDialogue.Count > 0)
        {
            player.GetComponent<ISpeaker>().StartSpeaking();
            if (enemy != null) enemy.GetComponent<ISpeaker>().StartSpeaking();
            skipText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IntroDialogue.Count < 1) return;
        if (introEnded == false)
        {
            if(mainIndex == 0)
            {
                IntroDialogue[mainIndex].gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SkipDialogue();
                AdvanceIndex();
                skipText.SetActive(false);
                skipText2.SetActive(false);
            }
            if (currentWaitTime < waitTime)
            {
                currentWaitTime += Time.deltaTime;                
            }
            else if (currentWaitTime >= waitTime)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
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
                if (enemyHp / 100 < 0.25 && subIndex == 2)
                {                    
                    StartCoroutine(ShowMessage(2));
                    subIndex++;
                }
                else if (enemyHp / 100 < 0.50 && subIndex == 1)
                {
                    StartCoroutine(ShowMessage(1));
                    subIndex++;
                }
                else if (enemyHp / 100 < 0.75 && subIndex ==0)
                {
                    StartCoroutine(ShowMessage(0));
                    subIndex++;
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
        if (mainIndex < IntroDialogue.Count - 1)
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
            if(screenBorders!= null)
            {
                screenBorders.SetActive(true);
            }
            if(panel != null)
            {
                panel.SetActive(false);
            }           
            skipText.SetActive(false);
            skipText2.SetActive(false);
        }

        if (mainIndex > 1 && mainIndex < 3)
        {
            skipText.SetActive(false);
            skipText2.SetActive(true);
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

            //yield return null;

        StopCoroutine(ShowMessage(index));
    }

    private void SkipDialogue()
    {
        foreach (var message in IntroDialogue)
        {
            message.SetActive(false);
        }
        mainIndex = IntroDialogue.Count-2;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void AddIntroMessage(GameObject message)
    {
        IntroDialogue.Add(message);
    }


}
