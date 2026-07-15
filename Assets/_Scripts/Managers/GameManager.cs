using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool levelEnded;

    [field: SerializeField] public PlayerController PlayerInstance {  get; private set; }

    [field: SerializeField] public bool IsGameActive { get; private set; }

    [SerializeField] private PlayerInput playerInput;

    [Header("Game End Screens")]
    [SerializeField] private MenuPage winScreen;
    [SerializeField] private MenuPage loseScreen;

    [Header("Analytics")]
    [SerializeField] private int analyticsLevelId = 1;
    [SerializeField] private string analyticsLevelName = "argentina";

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

        playerInput = GameInputManager.Instance.gameObject.GetComponent<PlayerInput>();
        levelEnded = false;
    }

    public void ToggleGameActiveState(bool state) => IsGameActive = state;

    public void OnGameWon()
    {
        if (levelEnded) return;
        levelEnded = true;
        FinalizeCurrentCombo("level_completed");
        Debug.Log("GAME WON");

        int currentAttempt =
    LevelAttemptTracker.Instance != null
        ? LevelAttemptTracker.Instance.CurrentAttempt
        : 1;

        string selectedWeaponId =
            GetSelectedWeaponAnalyticsId();

        LevelStarsTracker tracker =
            LevelStarsTracker.Instance;

        if (AnalyticsManager.Instance != null &&
            tracker != null)
        {
            AnalyticsManager.Instance.SendLevelCompleteEvent(
                analyticsLevelId,
                analyticsLevelName,
                selectedWeaponId,
                currentAttempt,
                tracker.ElapsedTime,
                tracker.TotalAttacks,
                tracker.GoodHits,
                tracker.PerfectHits,
                tracker.MissedHits,
                tracker.MaxCombo,
                tracker.DamageReceived,
                tracker.HealthRemaining
            );
        }
        else
        {
            Debug.LogWarning(
                "No se encontró AnalyticsManager o LevelStarsTracker."
            );
        }

        GameEnd();
        winScreen.OpenMenu();
    }

    public void OnGameLost()
    {
        if (levelEnded) return;
        levelEnded = true;
        Debug.Log("GAME LOST");

        string selectedWeaponId =
            GetSelectedWeaponAnalyticsId();

        LevelStarsTracker tracker =
            LevelStarsTracker.Instance;

        int currentAttempt =
            LevelAttemptTracker.Instance != null
                ? LevelAttemptTracker.Instance.CurrentAttempt
                : 1;

        if (AnalyticsManager.Instance != null &&
            tracker != null)
        {
            AnalyticsManager.Instance.SendLevelFailedEvent(
                analyticsLevelId,
                analyticsLevelName,
                selectedWeaponId,
                currentAttempt,
                tracker.ElapsedTime,
                tracker.TotalAttacks,
                tracker.GoodHits,
                tracker.PerfectHits,
                tracker.MissedHits,
                tracker.MaxCombo,
                tracker.DamageReceived
            );
        }
        else
        {
            Debug.LogWarning(
                "No se encontró AnalyticsManager o LevelStarsTracker."
            );
        }

        GameEnd();
        loseScreen.OpenMenu();
    }

    private void GameEnd()
    {
        IsGameActive = false;
        

        Time.timeScale = 1;
    }

    public void MakePlayerInvincible()
    {
        BeatManager.Instance.ToggleMusic(false);
        PlayerInstance.Health.ToggleInvincibility(true);
        playerInput.SwitchCurrentActionMap("UI");
    }

    [SerializeField]
    private string[] analyticsWeaponIds =
{
    "microphone",
    "accordion",
    "saxophone"
};

    private string GetSelectedWeaponAnalyticsId()
    {
        if (DataPersistanceManager.Instance == null)
        {
            Debug.LogWarning("No se encontró DataPersistanceManager.");
            return "unknown";
        }

        int weaponIndex = DataPersistanceManager.Instance.GetSelectedWeapon();

        if (weaponIndex < 0 || weaponIndex >= analyticsWeaponIds.Length)
        {
            Debug.LogWarning($"Índice de arma desconocido: {weaponIndex}");
            return $"weapon_{weaponIndex}";
        }

        return analyticsWeaponIds[weaponIndex];
    }

    public void OnLevelAbandoned(string abandonReason)
    {
        if (levelEnded)
        {
            return;
        }

        levelEnded = true;
        FinalizeCurrentCombo("level_abandoned");

        string selectedWeaponId =
            GetSelectedWeaponAnalyticsId();

        LevelStarsTracker tracker =
            LevelStarsTracker.Instance;

        int currentAttempt =
            LevelAttemptTracker.Instance != null
                ? LevelAttemptTracker.Instance.CurrentAttempt
                : 1;

        if (AnalyticsManager.Instance != null &&
            tracker != null)
        {
            AnalyticsManager.Instance.SendLevelAbandonedEvent(
                analyticsLevelId,
                analyticsLevelName,
                selectedWeaponId,
                currentAttempt,
                tracker.ElapsedTime,
                abandonReason,
                tracker.TotalAttacks,
                tracker.GoodHits,
                tracker.PerfectHits,
                tracker.MissedHits,
                tracker.MaxCombo,
                tracker.DamageReceived,
                tracker.HealthRemaining
            );
        }
        else
        {
            Debug.LogWarning(
                "No se encontró AnalyticsManager o LevelStarsTracker."
            );
        }

        GameEnd();
    }

    private void FinalizeCurrentCombo(string reason)
    {
        if (PlayerInstance == null)
        {
            return;
        }

        BeatComboCounter comboCounter =
            PlayerInstance.GetComponent<BeatComboCounter>();

        if (comboCounter != null)
        {
            comboCounter.FinalizeAnalyticsCombo(reason);
        }
    }
}
