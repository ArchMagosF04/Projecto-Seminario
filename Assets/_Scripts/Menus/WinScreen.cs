using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public void LoadLvlSelection()
    {
        SceneManager.LoadScene("Lvl Selection Screen");
    }
    
    public void LoadOtherLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
