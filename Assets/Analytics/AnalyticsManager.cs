using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using static LevelCompleteEvent;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("testing");

            await UnityServices.InitializeAsync(options);

            AnalyticsService.Instance.StartDataCollection();

            IsReady = true;

            Debug.Log("UNITY ANALYTICS INICIALIZADO");
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"No se pudo inicializar Unity Analytics: {exception.Message}"
            );
        }
    }

    public void SendLevelStartEvent(
        int levelId,
        string levelName,
        string weaponId,
        int attemptNumber,
        bool isTutorial)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "LEVEL_START no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        LevelStartEvent levelStartEvent = new LevelStartEvent
        {
            LevelId = levelId,
            LevelName = levelName,
            WeaponId = weaponId,
            AttemptNumber = attemptNumber,
            IsTutorial = isTutorial
        };

        AnalyticsService.Instance.RecordEvent(levelStartEvent);

        Debug.Log(
            $"LEVEL_START enviado | " +
            $"ID: {levelId} | " +
            $"Nivel: {levelName} | " +
            $"Arma: {weaponId} | " +
            $"Intento: {attemptNumber} | " +
            $"Tutorial: {isTutorial}"
        );
    }

    public void SendLevelCompleteEvent(
    int levelId,
    string levelName,
    string weaponId,
    int attemptNumber,
    float completionTimeSeconds,
    int totalAttacks,
    int goodHits,
    int perfectHits,
    int missedHits,
    int maxCombo,
    int damageReceived,
    int healthRemaining)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "LEVEL_COMPLETE no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        LevelCompleteEvent levelCompleteEvent = new LevelCompleteEvent
        {
            LevelId = levelId,
            LevelName = levelName,
            WeaponId = weaponId,
            AttemptNumber = attemptNumber,
            CompletionTimeSeconds = completionTimeSeconds,
            TotalAttacks = totalAttacks,
            GoodHits = goodHits,
            PerfectHits = perfectHits,
            MissedHits = missedHits,
            MaxCombo = maxCombo,
            DamageReceived = damageReceived,
            HealthRemaining = healthRemaining
        };

        AnalyticsService.Instance.RecordEvent(levelCompleteEvent);

        Debug.Log(
            $"LEVEL_COMPLETE enviado | Nivel: {levelName} | " +
            $"Arma: {weaponId} | Intento: {attemptNumber} | " +
            $"Tiempo: {completionTimeSeconds:F2}s | Ataques: {totalAttacks} | " +
            $"Good: {goodHits} | Perfect: {perfectHits} | Fallados: {missedHits}"
        );
    }

    public void SendLevelFailedEvent(
    int levelId,
    string levelName,
    string weaponId,
    int attemptNumber,
    float failureTimeSeconds,
    int totalAttacks,
    int goodHits,
    int perfectHits,
    int missedHits,
    int maxCombo,
    int damageReceived)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "LEVEL_FAILED no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        LevelFailedEvent levelFailedEvent = new LevelFailedEvent
        {
            LevelId = levelId,
            LevelName = levelName,
            WeaponId = weaponId,
            AttemptNumber = attemptNumber,
            FailureTimeSeconds = failureTimeSeconds,
            TotalAttacks = totalAttacks,
            GoodHits = goodHits,
            PerfectHits = perfectHits,
            MissedHits = missedHits,
            MaxCombo = maxCombo,
            DamageReceived = damageReceived
        };

        AnalyticsService.Instance.RecordEvent(levelFailedEvent);

        Debug.Log(
            $"LEVEL_FAILED enviado | Nivel: {levelName} | " +
            $"Arma: {weaponId} | Intento: {attemptNumber} | " +
            $"Tiempo: {failureTimeSeconds:F2}s | Ataques: {totalAttacks} | " +
            $"Good: {goodHits} | Perfect: {perfectHits} | " +
            $"Fallados: {missedHits} | Max Combo: {maxCombo}"
        );
    }

    public void SendWeaponSelectedEvent(
    string weaponId,
    int weaponIndex,
    string previousWeaponId,
    bool changedWeapon)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "WEAPON_SELECTED no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        WeaponSelectedEvent weaponSelectedEvent = new WeaponSelectedEvent
        {
            WeaponId = weaponId,
            WeaponIndex = weaponIndex,
            PreviousWeaponId = previousWeaponId,
            ChangedWeapon = changedWeapon
        };

        AnalyticsService.Instance.RecordEvent(weaponSelectedEvent);

        Debug.Log(
            $"WEAPON_SELECTED enviado | Arma: {weaponId} | " +
            $"Índice: {weaponIndex} | Anterior: {previousWeaponId} | " +
            $"Cambió: {changedWeapon}"
        );
    }

    public void SendWeaponSelectorOpenedEvent(
    int levelId,
    string levelName,
    string weaponId)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "WEAPON_SELECTOR_OPENED no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        WeaponSelectorOpenedEvent selectorOpenedEvent =
            new WeaponSelectorOpenedEvent
            {
                LevelId = levelId,
                LevelName = levelName,
                WeaponId = weaponId
            };

        AnalyticsService.Instance.RecordEvent(selectorOpenedEvent);

        Debug.Log(
            $"WEAPON_SELECTOR_OPENED enviado | " +
            $"Nivel: {levelName} | Arma actual: {weaponId}"
        );
    }
    public void SendLevelAbandonedEvent(
    int levelId,
    string levelName,
    string weaponId,
    int attemptNumber,
    float abandonmentTimeSeconds,
    string abandonReason,
    int totalAttacks,
    int goodHits,
    int perfectHits,
    int missedHits,
    int maxCombo,
    int damageReceived,
    int healthRemaining)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "LEVEL_ABANDONED no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        LevelAbandonedEvent levelAbandonedEvent = new LevelAbandonedEvent
        {
            LevelId = levelId,
            LevelName = levelName,
            WeaponId = weaponId,
            AttemptNumber = attemptNumber,
            AbandonmentTimeSeconds = abandonmentTimeSeconds,
            AbandonReason = abandonReason,
            TotalAttacks = totalAttacks,
            GoodHits = goodHits,
            PerfectHits = perfectHits,
            MissedHits = missedHits,
            MaxCombo = maxCombo,
            DamageReceived = damageReceived,
            HealthRemaining = healthRemaining
        };

        AnalyticsService.Instance.RecordEvent(levelAbandonedEvent);

        Debug.Log(
            $"LEVEL_ABANDONED enviado | Nivel: {levelName} | " +
            $"Arma: {weaponId} | Intento: {attemptNumber} | " +
            $"Tiempo: {abandonmentTimeSeconds:F2}s | " +
            $"Motivo: {abandonReason} | Ataques: {totalAttacks}"
        );
    }

    public void SendComboResultEvent(
    int levelId,
    string levelName,
    string weaponId,
    int attemptNumber,
    string comboOutcome,
    int comboLength,
    int comboThreshold,
    string breakReason)
    {
        if (!IsReady)
        {
            Debug.LogWarning(
                "COMBO_RESULT no se envió porque Analytics todavía no está listo."
            );

            return;
        }

        ComboResultEvent comboResultEvent = new ComboResultEvent
        {
            LevelId = levelId,
            LevelName = levelName,
            WeaponId = weaponId,
            AttemptNumber = attemptNumber,
            ComboOutcome = comboOutcome,
            ComboLength = comboLength,
            ComboThreshold = comboThreshold,
            BreakReason = breakReason
        };

        AnalyticsService.Instance.RecordEvent(comboResultEvent);

        Debug.Log(
            $"COMBO_RESULT enviado | Nivel: {levelName} | " +
            $"Arma: {weaponId} | Intento: {attemptNumber} | " +
            $"Resultado: {comboOutcome} | Combo: {comboLength} | " +
            $"Objetivo: {comboThreshold} | Motivo: {breakReason}"
        );
    }

}

