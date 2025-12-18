using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AutoDialogueManager : MonoBehaviour
{    
    [SerializeField] private GameObject player;
    private DataPersistanceManager persistanceManager;
    [SerializeField] TMPro.TMP_Text dialogue;
    private PlayerWeapon weaponScript;
    private int selectedWeapon;


    [SerializeField] private List<string> IntroDialogue;
    [SerializeField]private float waitTime = 0.30f;
    private float currentWaitTime = 0;

    private bool paused = false;
    //private bool messageOnScreen = false;

    private int mainIndex = -1;
    private int subIndex = 0;


    public bool manualWeaponOverride;
    public int manualOverrideWeaponIndex;


    // Start is called before the first frame update
    private void Awake()
    {        
        //holster.OnWeaponLoaded += GetSelectedWeapon;
        persistanceManager = GameObject.Find("DataPersistanceManager").GetComponent<DataPersistanceManager>();
        if (manualWeaponOverride)
        {
            persistanceManager.ChangeSelectedWeapon(manualOverrideWeaponIndex);
        }
        selectedWeapon = persistanceManager.GetSelectedWeapon();
    }

    void Start()
    {
        weaponScript = player.GetComponent<PlayerController>().weapon;
        SetDialogueTriggers(selectedWeapon);
        AdvanceIndex();

    }

    // Update is called once per frame
    void Update()
    {
        //if(IntroDialogue.Count < 1 || paused) return;        

        //    if (currentWaitTime < waitTime)
        //    {
        //        currentWaitTime += Time.deltaTime;                
        //    }
        //    else if (currentWaitTime >= waitTime)
        //    {
        //        if (/*condition to advance*/true)
        //        {
        //            currentWaitTime = 0;
        //            AdvanceIndex();

        //        }                
        //    }      
    }

    [ContextMenu("AdvanceIndex")]
    public void AdvanceIndex()
    {
        if (TextSelector(selectedWeapon,mainIndex+1) != "" || mainIndex == -1)
        {
            mainIndex++;
            dialogue.text = TextSelector(selectedWeapon, mainIndex);
            Debug.Log(mainIndex);
            //if (!paused) IntroDialogue[mainIndex].gameObject.SetActive(true);
        }
        else
        {
            mainIndex = 0;
            dialogue.text = TextSelector(selectedWeapon, mainIndex);
        }

    }    

    //public void ShowMessage(int index)
    //{
    //    MidCombatDialogue[index].gameObject.SetActive(true);
    //}

    //public void Pause()
    //{
    //    paused = true;
    //    IntroDialogue[mainIndex].gameObject.SetActive(false);
    //    IntroDialogue[mainIndex+1].gameObject.SetActive(false);
        
    //    player.GetComponent<ISpeaker>().StopSpeaking();
    //    this.gameObject.SetActive(false);
    //}

    //public void Unpause()
    //{
    //    paused = false;       
    //    IntroDialogue[mainIndex].gameObject.SetActive(true);       
    //    player.GetComponent<ISpeaker>().StartSpeaking();
    //    this.gameObject.SetActive(true);
    //}

    private void GetSelectedWeapon(int weaponIndex)
    {
        selectedWeapon = weaponIndex;
    }

    private string TextSelector(int weapon, int index)
    {
        switch (weapon)
        {
            case 0: return "";
                
            case 1:
                switch (index)
                {
                    case 0:
                        dialogue.color = Color.white;
                        dialogue.fontSize = 20;
                        return "HOLD the Atack Button to charge up your attack and increase it's damage and speed";

                    case 1:
                        dialogue.color = Color.cyan;
                        return "Press the special attack button to enter the special mode";

                    case 2:
                        dialogue.color = Color.yellow;
                        dialogue.fontSize = 18;
                        return "While the special mode is active, normal atacks become Fully Charged atacks and you don't need to charge them!";                        
                }
            break;

            default: return "";
        }
        return "";
    }

    private void SetDialogueTriggers(int weapon)
    {
        switch (weapon)
        {
            case 0:
                break;
            case 1:
                Core_Mana.ManaIsFull += AdvanceIndex;
                weaponScript.OnSpecialEnter += AdvanceIndex;
                PW_Accordion.OnspecialEnded += AdvanceIndex;
                break;
        }
    }

}
