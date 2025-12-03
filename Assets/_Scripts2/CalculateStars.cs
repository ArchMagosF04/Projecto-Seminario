using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CalculateStars : MonoBehaviour
{
    private float elapsedTime = 0;
    [SerializeField] private BeatComboCounter comboManager;
    [SerializeField] private Core_Health playerHealth;
    private bool countTime;
    [SerializeField] private StarTracker starsUI;
    //[SerializeField] GameManager gameManager;

    public event System.Action<bool[]> OnResultsCalculated = delegate { };

    [Tooltip ("Cual es la cantidad maxima de tiempo que el jugador puede tardar")]
    [SerializeField] private float TargetTime;

    private float remainingHealth;
    [Tooltip ("Cual es la cantidad minima de vida que el jugador debe mantener al terminar el nivel")]
    [SerializeField] private float TargetHealth;

    private int maxCombo;
    [Tooltip ("El combo maximo que debe alcanzar el jugador")]
    [SerializeField] private int ComboTarget;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        if (countTime) { elapsedTime = +Time.deltaTime; }
    }

    public void GetResults()
    {
        string result;
        countTime = false;
        remainingHealth = playerHealth.CurrentHealth;
        maxCombo = comboManager.maxCombo;

        //gameManager.OnWinGame();

        if (elapsedTime <= TargetTime)
        {
            result = "1";
        }
        else 
        {
            result = "0";
        }

        if(remainingHealth >= TargetHealth)
        {
            result = result + "1";
        }
        else
        {
            result = result + "0";
        }
        if (maxCombo >= ComboTarget)
        {
            result = result + "1";
        }
        else
        {
            result = result + "0";
        }

        string lvlName = GlobalManager.Instance.GetCurrentLevel();
        string variableName = lvlName + "Stars";



        if (PlayerPrefs.GetString("variableName") != null && PlayerPrefs.GetString("variableName")!="")
        {
            bool[] oldResult = StarsStringDecoder.DecodeString(PlayerPrefs.GetString("variableName"));
            bool[] newResult = StarsStringDecoder.DecodeString(result);           

            for (int i= 0; i < oldResult.Length; i++)
            {
                if (oldResult[i] == true && newResult[i]== false)
                {
                    newResult[i] = true;
                }
            }            

            result = "";

            foreach(bool flag in newResult)
            {
                if(flag==true) result += "1";
                else result += "0";
            }

            PlayerPrefs.SetString(variableName, result);

            OnResultsCalculated(newResult);

            Destroy(gameObject);
        }
        else
        {
            PlayerPrefs.SetString(variableName, result);
            bool[] newResult = StarsStringDecoder.DecodeString(result);
            OnResultsCalculated(newResult);
            Destroy(gameObject);

        }        
    }





}
