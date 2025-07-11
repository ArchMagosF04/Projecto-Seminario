using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine;
using System;

public class LvlSelectionController : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] float cameraSpeed = 3;
    [SerializeField] GameObject[] worldMarkers;
    [SerializeField] String[] levels;
    private int index = 0;

    private bool levelSelected = false;
    public static Action<bool> OnSelection = delegate { };    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(index < worldMarkers.Length-1)
            {
                index += 1;
            }
            else index = 0;
            
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(index > 0)
            {
                index -= 1;
            }
            else index = worldMarkers.Length-1;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (index < levels.Length && levels[index] != null)
            {
                levelSelected = true;
                OnSelection(levelSelected);
            }
            else
            {
                Debug.Log("Level slot is empty");
            }
        }

        if(levelSelected && Input.GetKeyDown(KeyCode.Escape))
        {
            levelSelected=false;
            OnSelection(levelSelected);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnSelection = delegate { };
            Destroy(GameObject.Find("Music"));
            SceneManager.LoadScene("Main Menu");
            SceneManager.UnloadSceneAsync("Lvl Selection Screen");
        }

        if (camera.transform.position != worldMarkers[index].transform.position)
        {
            Vector2 temp = (worldMarkers[index].transform.position - camera.transform.position).normalized;
            camera.transform.Translate(temp * Time.deltaTime * cameraSpeed);
        }
    }

    public void LoadLevel()
    {
        if (levels[index] != null)
        {
            OnSelection = delegate { };
            SceneManager.LoadScene(levels[index]);
        }        
    }
}
