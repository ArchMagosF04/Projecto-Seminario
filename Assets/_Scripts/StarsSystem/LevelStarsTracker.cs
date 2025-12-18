using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStarsTracker : MonoBehaviour, IDataPersistance
{
    public static LevelStarsTracker Instance;

    [SerializeField] private string levelID = "1";

    [Header("Stars Requirements")]
    [SerializeField, Range(0, 100)] private int maxComboRequired = 10;
    [SerializeField, Range(0f, 1f)] private float healthPercentageRequired = 0.75f;
    [SerializeField, Range(0, 300f)] private float timeRequired = 120;

    [Header("BeatCombo Tracking")]
    [SerializeField] private BeatComboCounter playerComboCounter;

    [Header("Damage Tracking")]
    [SerializeField] private Core_Health playerHealth;

    [Header("Time Tracking")]
    [SerializeField] private TMP_Text timerText;

    [Header("UI Elements")]
    [SerializeField] private GameObject previousStarA;
    [SerializeField] private GameObject unlockedStarA;
    [SerializeField] private TMP_Text starAText;
    [SerializeField] private TMP_Text achievedTextA;

    [Space(3)]

    [SerializeField] private GameObject previousStarB;
    [SerializeField] private GameObject unlockedStarB;
    [SerializeField] private TMP_Text starBText;
    [SerializeField] private TMP_Text achievedTextB;

    [Space(3)]

    [SerializeField] private GameObject previousStarC;
    [SerializeField] private GameObject unlockedStarC;
    [SerializeField] private TMP_Text starCText;
    [SerializeField] private TMP_Text achievedTextC;

    [Space(3)]

    private float elapseTime;

    private bool hasPreviouslyAchievedComboRequirements;
    private bool hasPreviouslyAchievedDamageRequirements;
    private bool hasPreviouslyAchievedTimeRequirements;

    private bool passedComboRequirements;
    private bool passedDamageRequirements;
    private bool passedTimeRequirements;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        playerComboCounter = GameManager.Instance.PlayerInstance.gameObject.GetComponent<BeatComboCounter>();
        playerHealth = GameManager.Instance.PlayerInstance.gameObject.GetComponentInChildren<Core_Health>();

        SetUpStars();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            elapseTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapseTime / 60);
            int seconds = Mathf.FloorToInt(elapseTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void SetUpStars()
    {
        previousStarA.SetActive(false);
        previousStarB.SetActive(false);
        previousStarC.SetActive(false);
        unlockedStarA.SetActive(false);
        unlockedStarB.SetActive(false);
        unlockedStarC.SetActive(false);

        starAText.text = "Reach a Combo score of " + maxComboRequired;
        starBText.text = "Beat the boss with over " + healthPercentageRequired * 100f + " health";

        int minutes = Mathf.FloorToInt(timeRequired / 60);
        int seconds = Mathf.FloorToInt(timeRequired % 60);

        starCText.text = "Complete the level in less than " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CalculateResults()
    {
        achievedTextA.text = playerComboCounter.MaxCombo.ToString();
        achievedTextB.text = (playerHealth.GetCurrentHealthPercentage() * 100).ToString() + "%";
        achievedTextC.text = timerText.text;

        if (playerComboCounter.MaxCombo >= maxComboRequired) passedComboRequirements = true;

        if (playerHealth.GetCurrentHealthPercentage() >= healthPercentageRequired) passedDamageRequirements = true;

        if (elapseTime <= timeRequired) passedTimeRequirements = true;
    }

    public void ShowStars()
    {
        if (passedComboRequirements) unlockedStarA.SetActive(true);
        else if (hasPreviouslyAchievedComboRequirements) previousStarA.SetActive(true);

        if (passedDamageRequirements) unlockedStarB.SetActive(true);
        else if (hasPreviouslyAchievedDamageRequirements) previousStarB.SetActive(true);

        if (passedTimeRequirements) unlockedStarC.SetActive(true);
        else if (hasPreviouslyAchievedTimeRequirements) previousStarC.SetActive(true);
    }

    public void LoadData(GameData gameData)
    {
        string starA = "Star-" + levelID + "A";
        string starB = "Star-" + levelID + "B";
        string starC = "Star-" + levelID + "C";

        if (gameData.starsGained.ContainsKey(starA))
        {
            hasPreviouslyAchievedComboRequirements = gameData.starsGained[starA];
        }
        else Debug.LogError("Wrong Level Key");

        if (gameData.starsGained.ContainsKey(starB))
        {
            hasPreviouslyAchievedDamageRequirements = gameData.starsGained[starB];
        }
        else Debug.LogError("Wrong Level Key");

        if (gameData.starsGained.ContainsKey(starC))
        {
            hasPreviouslyAchievedTimeRequirements = gameData.starsGained[starC];
        }
        else Debug.LogError("Wrong Level Key");

        MenuBackground.backgroundIndex = PlayerPrefs.GetInt("background");
    }

    public void SaveData(GameData gameData)
    {
        string starA = "Star-" + levelID + "A";
        string starB = "Star-" + levelID + "B";
        string starC = "Star-" + levelID + "C";

        if (passedComboRequirements)
        {
            if (gameData.starsGained.ContainsKey(starA))
            {
                gameData.starsGained[starA] = true;
            }
            else Debug.LogError("Wrong Level Key");
        }

        if (passedDamageRequirements)
        {
            if (gameData.starsGained.ContainsKey(starB))
            {
                gameData.starsGained[starB] = true;
            }
            else Debug.LogError("Wrong Level Key");
        }

        if (passedTimeRequirements)
        {
            if (gameData.starsGained.ContainsKey(starC))
            {
                gameData.starsGained[starC] = true;
            }
            else Debug.LogError("Wrong Level Key");
        }

        switch (levelID)
        {
            case "1":

                gameData.unlocked2ndLevel = true;
                gameData.unlockedweapon2 = true;
                MenuBackground.backgroundIndex = 2;
                PlayerPrefs.SetInt("background", 2);

                break;
            case "2":

                gameData.unlocked3rdLevel = true;
                gameData.unlockedweapon3 = true;
                MenuBackground.backgroundIndex = 3;
                PlayerPrefs.SetInt("background", 3);
                break;
            case "3":

                gameData.unlockedweapon4 = true;
                break;
            default:

                Debug.LogError("Wrong Level ID", this);
                break;
        }
    }
}
