using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLvlSelection(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowOptionsMenu()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
