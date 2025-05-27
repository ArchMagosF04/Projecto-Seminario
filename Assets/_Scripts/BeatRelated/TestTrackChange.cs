using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrackChange : MonoBehaviour
{
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private TrackInfo newTrack;
    [SerializeField] private float timeToChangeTrack;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timeToChangeTrack) 
        {
            timer += Time.deltaTime;
        }
        else if (timer >= timeToChangeTrack)
        {
            beatManager.ChangeTrack(newTrack);
            Destroy(gameObject);
        }
        
    }
}
