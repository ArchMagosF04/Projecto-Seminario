using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject VolumeOptions;
    [SerializeField] List<GameObject> Buttons = new List<GameObject>();
    public void ReturnGame()
    {
        GameManager.Instance.PauseMenu(false);
    }

    public void Volume()
    {
        VolumeOptions.SetActive(true);
        ToggleMenuButtons(false);
    }
    public void VolumeBack()
    {
        VolumeOptions.SetActive(false);
        ToggleMenuButtons(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private void ToggleMenuButtons(bool toggle)
    {
        foreach (var button in Buttons)
        {
            button.SetActive(toggle);
        }
    }

}
