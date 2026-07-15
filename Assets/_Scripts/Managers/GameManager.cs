using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public PlayerController PlayerInstance {  get; private set; }

    [field: SerializeField] public bool IsGameActive { get; private set; }

    [SerializeField] private PlayerInput playerInput;

    [Header("Game End Screens")]
    [SerializeField] private MenuPage winScreen;
    [SerializeField] private MenuPage loseScreen;

    [Header("Analytics")]
    [SerializeField] private int analyticsLevelId = 1;
    [SerializeField] private string analyticsLevelName = "argentina";
    [SerializeField] private int analyticsAttemptNumber = 1;

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
    }

    public void ToggleGameActiveState(bool state) => IsGameActive = state;

    public void OnGameWon()
    {
        Debug.Log("GAME WON");
        string selectedWeaponId = GetSelectedWeaponAnalyticsId();
        BeatComboCounter comboCounter =
    PlayerInstance.GetComponentInChildren<BeatComboCounter>();

        int maxCombo = comboCounter != null
            ? comboCounter.MaxCombo
            : 0;

        int damageReceived =
            Mathf.RoundToInt(PlayerInstance.Health.TotalDamageReceived);

        int healthRemaining =
            Mathf.RoundToInt(PlayerInstance.Health.CurrentHealth);

        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendLevelCompleteEvent(
                analyticsLevelId,
                analyticsLevelName,
                selectedWeaponId,
                analyticsAttemptNumber,
                Time.timeSinceLevelLoad,
                20, // total_attacks - temporal
                10, // good_hits - temporal
                5,  // perfect_hits - temporal
                5,  // missed_hits - temporal
                maxCombo,
                damageReceived,
                healthRemaining
            );
        }
        else
        {
            Debug.LogWarning(
                "No se encontró AnalyticsManager al completar el nivel."
            );
        }

        GameEnd();
        winScreen.OpenMenu();
    }

    public void OnGameLost()
    {
        Debug.Log("GAME LOST");
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
}
