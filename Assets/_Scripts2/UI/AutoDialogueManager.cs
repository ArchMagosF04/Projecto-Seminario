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
    public int SelectedWeapon {  get { return selectedWeapon; }}


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
            case 0:
                switch (index)
                {
                    case 0:
                        dialogue.color = Color.black;
                        dialogue.fontSize = 10;
                        return "Press the Attack Button to throw the Mic foward.\n Press W + Attack Button or S + Attack Button to launch the Mic up or down.";

                    case 1:
                        dialogue.color = Color.red;
                        return "Press the special attack button to swing the Microphone at the enemy 3 times.";
                }
                break;

            case 1:
                switch (index)
                {
                    case 0:
                        dialogue.color = Color.black;
                        dialogue.fontSize = 9;
                        return "HOLD the Attack Button to charge up the attack and increase it's damage and speed.\n\n The weapon has 2 charge levels (first green then pink when fully charged.)";

                    case 1:
                        dialogue.color = Color.red;
                        return "Press the special attack button to enter the special mode.";

                    case 2:
                        dialogue.color = Color.blue;
                        dialogue.fontSize = 10;
                        return "While the special mode is active, normal attacks become Fully Charged attacks and there's no need to charge them!";                        
                }
            break;

            case 2:
                switch (index)
                {
                    case 0:
                        dialogue.color = Color.black;
                        dialogue.fontSize = 9;
                        return "Press the Attack Button to fire a note in an arc.\n\nIf the note is fired at the right time, it will stick to the enemy and explode after some time.";

                    case 1:
                        dialogue.color = Color.red;
                        dialogue.fontSize = 12;
                        return "Press the special attack button to cause a damage explotion around the weapon.";

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
                Core_Mana.ManaIsFull += AdvanceIndex;
                weaponScript.OnSpecialEnter += AdvanceIndex;
                break;
            case 1:
                Core_Mana.ManaIsFull += AdvanceIndex;
                weaponScript.OnSpecialEnter += AdvanceIndex;
                PW_Accordion.OnspecialEnded += AdvanceIndex;
                break;

            case 2:
                Core_Mana.ManaIsFull += AdvanceIndex;
                weaponScript.OnSpecialEnter += AdvanceIndex;
                break;
        }
    }

}
