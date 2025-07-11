using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject options;

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
        options.SetActive(true);
    }

    public void OptionsBack()
    {
        options.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }
     public void CreditsBack()
    {
        mainMenu.SetActive(true);
        credits.SetActive(false);
    }
}
