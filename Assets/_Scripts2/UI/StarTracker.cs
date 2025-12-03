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

    protected void Start()
    {
        if (showEmptyStars)
        {
            foreach(GameObject greyStar in starsSockets)
            {
                greyStar.SetActive(true);
            }
        }
    }

    public void AddStars(bool[] flags)
    {
        if (starsImages != null && flags.Length>0 && flags.Length <=3)
        {
            for(int i = 0; i< flags.Length; i++)
            {
                starsImages[i].SetActive(flags[i]);
            }
        }
        else return;
    }


}
