using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField, Tooltip("The gameObject that holds all the UI in the scene")] private GameObject sceneUIHolder;

    [SerializeField] private Image loadingBar;

    //Add field to disable input system.
    [SerializeField] private PlayerInput playerInput;

    public static AsyncSceneLoader Instance;

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

        loadingScreenUI.SetActive(false);
        loadingBar.fillAmount = 0;
    }

    public void LoadLevel(string sceneName)
    {
        //DisableMenus
        sceneUIHolder.SetActive(false);

        //Disable player input
        playerInput.enabled = false;

        //Activate loading Screen.
        loadingScreenUI.SetActive(true);

        //Save the game data in the scene before leaving.
        DataPersistanceManager.Instance.SaveGame();

        //Run the Async
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    public void LoadLevel(int sceneIndex)
    {
        //DisableMenus
        sceneUIHolder.SetActive(false);

        //Disable player input
        playerInput.enabled = false;

        //Activate loading Screen.
        loadingScreenUI.SetActive(true);

        //Save the game data in the scene before leaving.
        DataPersistanceManager.Instance.SaveGame();

        //Run the Async
        StartCoroutine(LoadLevelAsync(sceneIndex));
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        loadingBar.fillAmount = 0;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return null;
        }

        if (Time.timeScale < 1) Time.timeScale = 1;
    }

    private IEnumerator LoadLevelAsync(int sceneIndex)
    {
        loadingBar.fillAmount = 0;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return null;
        }

        if (Time.timeScale < 1) Time.timeScale = 1;
    }
}
