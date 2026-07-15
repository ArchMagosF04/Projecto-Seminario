using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAttemptTracker : MonoBehaviour
{
    public static LevelAttemptTracker Instance { get; private set; }

    private static readonly Dictionary<string, int> attemptsByScene =
        new Dictionary<string, int>();

    [Header("Analytics Level Data")]
    [SerializeField] private int analyticsLevelId = 1;
    [SerializeField] private string analyticsLevelName = "argentina";
    [SerializeField] private bool isTutorial = false;

    public int CurrentAttempt { get; private set; }
    public int AnalyticsLevelId => analyticsLevelId;
    public string AnalyticsLevelName => analyticsLevelName;
    public bool IsTutorial => isTutorial;

    private string currentSceneName;

    // Limpia los valores estáticos cada vez que inicia el juego.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticData()
    {
        attemptsByScene.Clear();
        Instance = null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentSceneName = SceneManager.GetActiveScene().name;

        int previousAttempts = 0;
        attemptsByScene.TryGetValue(currentSceneName, out previousAttempts);

        CurrentAttempt = previousAttempts + 1;
        attemptsByScene[currentSceneName] = CurrentAttempt;

        Debug.Log(
            $"INTENTO INICIADO | Escena: {currentSceneName} | " +
            $"Intento: {CurrentAttempt}"
        );
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            AnalyticsManager.Instance != null &&
            AnalyticsManager.Instance.IsReady
        );

        string weaponId = GetSelectedWeaponAnalyticsId();

        AnalyticsManager.Instance.SendLevelStartEvent(
            analyticsLevelId,
            analyticsLevelName,
            weaponId,
            CurrentAttempt,
            isTutorial
        );
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public static void ResetAttemptsForScene(string sceneName)
    {
        attemptsByScene[sceneName] = 0;

        Debug.Log($"INTENTOS REINICIADOS | Escena: {sceneName}");
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