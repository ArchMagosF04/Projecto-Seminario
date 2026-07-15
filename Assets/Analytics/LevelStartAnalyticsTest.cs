using System.Collections;
using UnityEngine;

public class LevelStartAnalyticsTest : MonoBehaviour
{
    [Header("Datos de prueba")]
    [SerializeField] private int levelId = 1;
    [SerializeField] private string levelName = "argentina";
    [SerializeField] private string weaponId = "microphone";
    [SerializeField] private int attemptNumber = 1;
    [SerializeField] private bool isTutorial;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            AnalyticsManager.Instance != null &&
            AnalyticsManager.Instance.IsReady
        );

        AnalyticsManager.Instance.SendLevelStartEvent(
            levelId,
            levelName,
            weaponId,
            attemptNumber,
            isTutorial
        );
    }
}