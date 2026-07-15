using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

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
            await UnityServices.InitializeAsync();

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
}

