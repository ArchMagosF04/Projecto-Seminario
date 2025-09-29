using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    private static SceneLoaderManager sceneManager;
    public static GlobalManager gManager;

    private void Start()
    {
        sceneManager = SceneLoaderManager.Instance;
        gManager = GlobalManager.Instance;
    }
    public void RetryLevel()
    {
        sceneManager.LoadSceneByName(gManager.GetCurrentScene());
    }
}
