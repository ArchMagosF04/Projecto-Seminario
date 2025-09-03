using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField]SoundLibraryObject library;

    // Start is called before the first frame update
    private void Awake()
    {
        library.Initialize();
    }

    void Start()
    {
        StartCoroutine(LateStart());
    }
    
    IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        SoundManager.Instance.CreateSound().WithSoundData(library.GetSound("CrowdCheersLoop")).Play();

        yield return null;
    }
}
