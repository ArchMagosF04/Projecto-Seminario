using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
    public static int backgroundIndex;
    [SerializeField] Sprite[] backgroundImages;
    private Image currentImage;
    // Start is called before the first frame update
    void Start()
    {
        currentImage = GetComponent<Image>();
        backgroundIndex = PlayerPrefs.GetInt("background");
        if(backgroundIndex>0)SetBackground(backgroundImages[backgroundIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("ChangeBackground")]
    private void ChangeBackground()
    {
        if(backgroundIndex+1 < backgroundImages.Length)
        {
            backgroundIndex++;
            SetBackground(backgroundImages[backgroundIndex]);
            currentImage.enabled = true;
            Debug.Log(backgroundIndex);
        }
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
        currentImage.enabled=false;
    }

    public void ShowBackground()
    {
        if(backgroundIndex!=0) currentImage.enabled = true;
    }
}
