using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTracker : MonoBehaviour
{
    private int starsAmount;
    public bool showEmptyStars;
    [SerializeField] GameObject[] starsSockets;
    [SerializeField] GameObject[] starsImages;
    // Start is called before the first frame update

    private void Start()
    {
        if (showEmptyStars)
        {
            foreach(GameObject greyStar in starsSockets)
            {
                greyStar.SetActive(true);
            }
        }
    }

    public void AddStars(int amount)
    {
        if (starsImages != null && amount>0 && amount <=3)
        {
            for(int i = 0; amount > i; i++)
            {
                starsImages[i].SetActive(true);
            }
        }
        else return;
    }


}
