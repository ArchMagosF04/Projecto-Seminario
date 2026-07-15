using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatComboCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterTextBox;
    [SerializeField] private Image rankIndicatorImage;
    [SerializeField] private SquashAndStretch comboIncreaseImage;
    [SerializeField] private StyleRank[] styleRanks;

    public StyleRank currentRank { get; private set; }

    private int beatComboCounter;
    private int currentRankIndex;

    public int MaxCombo { get; private set; }

    private Core core;
    private Core_Health health;

    private float timeUntilDecay;
    private float rankDuration;

    private Color workColor;

    [Header("Analytics")]
    [SerializeField, Min(1)]
    private int fallbackAnalyticsComboThreshold = 5;

    private bool analyticsComboActive;
    private int analyticsComboLength;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        health = core.GetCoreComponent<Core_Health>();
    }

    private void OnEnable()
    {
        health.OnDamageReceived += HandleDamageReceived;
        BeatManager.Instance.OnWrongBeat += HandleWrongBeat;
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDamageReceived -= HandleDamageReceived;
        }

        if (BeatManager.Instance != null)
        {
            BeatManager.Instance.OnWrongBeat -= HandleWrongBeat;
        }
    }

    private void Start()
    {
        MaxCombo = 0;
        beatComboCounter = 0;
        currentRankIndex = 0;
        currentRank = styleRanks[0];

        analyticsComboActive = false;
        analyticsComboLength = 0;

        if (rankIndicatorImage != null)
        {
            workColor = currentRank.rankColor;
            workColor.a = 1;
            rankIndicatorImage.color = workColor;
        }

        if (counterTextBox != null)
        {
            counterTextBox.text = beatComboCounter.ToString();
        }

        ResetDecayTimer();
    }

    private void Update()
    {
        ProgressTimer();

        if (rankIndicatorImage != null)
        {
            workColor = rankIndicatorImage.color;
            workColor.a = timeUntilDecay / rankDuration;
            rankIndicatorImage.color = workColor;
        }
    }

    [ContextMenu("Test Combo Increase")]
    public void IncreaseComboCounter(int amount)
    {
        if (amount > 0)
        {
            RegisterAnalyticsComboProgress(amount);
        }

        beatComboCounter += amount;

        if (comboIncreaseImage != null)
        {
            comboIncreaseImage.PlaySquashAndStretch();
        }

        if (MaxCombo < beatComboCounter)
        {
            MaxCombo = beatComboCounter;
        }

        if (counterTextBox != null)
        {
            counterTextBox.text = beatComboCounter.ToString();
        }

        if (currentRankIndex < styleRanks.Length - 1 &&
            beatComboCounter >=
            styleRanks[currentRankIndex + 1].rankThreshold)
        {
            currentRankIndex++;
            currentRank = styleRanks[currentRankIndex];
        }

        ResetDecayTimer();
    }

    public void ResetDecayTimer()
    {
        timeUntilDecay = GetRankDuration(currentRank);

        if (rankIndicatorImage != null)
        {
            workColor = currentRank.rankColor;
            workColor.a = 1;
            rankIndicatorImage.color = workColor;
        }
    }

    public float GetRankDuration(StyleRank rank)
    {
        float durationInBeats = rank.beatsForRankDecay;
        float beatDuration = 60f / BeatManager.Instance.BPM;

        rankDuration = durationInBeats * beatDuration;

        return rankDuration;
    }

    [ContextMenu("Test Timer Decay")]
    public void OnTimerDecay()
    {
        EndAnalyticsCombo("manual_decay");
        ApplyComboDecay();
    }

    private void HandleDamageReceived()
    {
        EndAnalyticsCombo("damage_received");
        ApplyComboDecay();
    }

    private void HandleWrongBeat()
    {
        EndAnalyticsCombo("wrong_beat");
        ApplyComboDecay();
    }

    private void HandleComboTimeout()
    {
        EndAnalyticsCombo("combo_timeout");
        ApplyComboDecay();
    }

    private void ApplyComboDecay()
    {
        if (currentRankIndex > 0)
        {
            currentRankIndex--;
            currentRank = styleRanks[currentRankIndex];
        }

        beatComboCounter = currentRank.rankThreshold;

        if (counterTextBox != null)
        {
            counterTextBox.text = beatComboCounter.ToString();
        }

        ResetDecayTimer();
    }

    private void ProgressTimer()
    {
        timeUntilDecay -= Time.deltaTime;

        if (timeUntilDecay <= 0)
        {
            HandleComboTimeout();
        }
    }

    public float GetDamageMultiplier()
    {
        return currentRank.rankDamageMultiplier;
    }

    public void FinalizeAnalyticsCombo(string reason)
    {
        EndAnalyticsCombo(reason);
    }

    private void RegisterAnalyticsComboProgress(int amount)
    {
        if (!analyticsComboActive)
        {
            analyticsComboActive = true;
            analyticsComboLength = 0;
        }

        analyticsComboLength += amount;
    }

    private void EndAnalyticsCombo(string breakReason)
    {
        if (!analyticsComboActive || analyticsComboLength <= 0)
        {
            return;
        }

        int comboThreshold = fallbackAnalyticsComboThreshold;

        if (LevelStarsTracker.Instance != null)
        {
            comboThreshold = Mathf.Max(
                1,
                LevelStarsTracker.Instance.ComboTarget
            );
        }

        string comboOutcome =
            analyticsComboLength >= comboThreshold
                ? "completed"
                : "broken";

        int levelId = -1;
        string levelName = "unknown";
        int attemptNumber = 1;

        if (LevelAttemptTracker.Instance != null)
        {
            levelId =
                LevelAttemptTracker.Instance.AnalyticsLevelId;

            levelName =
                LevelAttemptTracker.Instance.AnalyticsLevelName;

            attemptNumber =
                LevelAttemptTracker.Instance.CurrentAttempt;
        }

        string weaponId = GetSelectedWeaponAnalyticsId();

        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendComboResultEvent(
                levelId,
                levelName,
                weaponId,
                attemptNumber,
                comboOutcome,
                analyticsComboLength,
                comboThreshold,
                breakReason
            );
        }
        else
        {
            Debug.LogWarning(
                "No se encontró AnalyticsManager al finalizar el combo."
            );
        }

        analyticsComboActive = false;
        analyticsComboLength = 0;
    }

    private string GetSelectedWeaponAnalyticsId()
    {
        if (DataPersistanceManager.Instance == null ||
            !DataPersistanceManager.Instance.HasGameData())
        {
            return "unknown";
        }

        int weaponIndex =
            DataPersistanceManager.Instance.GetSelectedWeapon();

        switch (weaponIndex)
        {
            case 0:
                return "microphone";

            case 1:
                return "accordion";

            case 2:
                return "saxophone";

            case 3:
                return "weapon_4";

            default:
                return "unknown";
        }
    }
}