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
        SetBackground(backgroundImages[backgroundIndex]);
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
            Debug.Log(backgroundIndex);
        }
    }

    private void SetBackground(Sprite image)
    {
        if(image != null)
        {
            currentImage.sprite = image;
        }
    }
}
