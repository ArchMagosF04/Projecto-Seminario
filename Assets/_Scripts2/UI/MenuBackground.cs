using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour, IDataPersistance 
{
    public static int backgroundIndex;
    [SerializeField] Sprite[] backgroundImages;
    private Image currentImage;
    // Start is called before the first frame update

    private bool unlocked1stLevel;
    private bool unlocked2stLevel;
    private bool unlocked3rdLevel;

    void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    private void Start()
    {
        ChangeBackground();
    }

    [ContextMenu("ChangeBackground")]
    private void ChangeBackground()
    {
        if (unlocked3rdLevel)
        {
            SetBackground(backgroundImages[3]);
        }
        else if (unlocked2stLevel)
        {
            SetBackground(backgroundImages[2]);
        }
        else if (unlocked1stLevel)
        {
            SetBackground(backgroundImages[1]);
        }
        else
        {
            currentImage.enabled = false;
        }

        //if(backgroundIndex+1 < backgroundImages.Length)
        //{
        //    backgroundIndex++;
        //    SetBackground(backgroundImages[backgroundIndex]);
        //    currentImage.enabled = true;
        //    Debug.Log(backgroundIndex);
        //}
    }

    private void SetBackground(Sprite image)
    {
        if(image != null)
        {
            currentImage.enabled = true;
            currentImage.sprite = image;
        }
    }

    public void HideBackground()
    {
        if(currentImage != null) currentImage.enabled = false;
    }

    public void ShowBackground()
    {
        if(backgroundIndex != 0 && currentImage != null) currentImage.enabled = true;
    }

    public void LoadData(GameData gameData)
    {
        unlocked1stLevel = gameData.unlocked1stLevel;
        unlocked2stLevel = gameData.unlocked2ndLevel;
        unlocked3rdLevel = gameData.unlocked3rdLevel;
    }

    public void SaveData(GameData gameData)
    {
        
    }
}
