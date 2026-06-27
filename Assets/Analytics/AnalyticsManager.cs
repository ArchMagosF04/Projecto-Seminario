using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    async void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();

        DontDestroyOnLoad(gameObject);
    }

    public void SendNewGameEvent()
    {
        NewGameEvent newGame = new NewGameEvent();

        AnalyticsService.Instance.RecordEvent(newGame);
    }
}