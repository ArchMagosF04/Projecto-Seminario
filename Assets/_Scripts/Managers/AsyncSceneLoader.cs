using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject[] sceneMenus;

    [SerializeField] private Slider loadingSlider;

    public void LoadLevel(string sceneName)
    {
        //DisableMenus
        foreach (var sceneMenu in sceneMenus) sceneMenu.SetActive(false);

        //Activate loading Screen.
        loadingScreen.SetActive(true);

        //Run the Async
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }

        if (Time.timeScale < 1) Time.timeScale = 1;
    }
}
