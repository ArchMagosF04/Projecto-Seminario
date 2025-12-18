using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNeighbor : MonoBehaviour
{
    [SerializeField] GameObject sprite;
    [SerializeField] PushPlatform pushPlatform;
    [SerializeField] float showTime = 2;
    [SerializeField] private SoundID neighbourSound;

    // Start is called before the first frame update
    void Start()
    {
        sprite.SetActive(false);
        pushPlatform.OnPushPlayer += ShowSprite;
    }

    private void ShowSprite()
    {
        sprite.SetActive(true);
        if (neighbourSound.IsValid()) BroAudio.Play(neighbourSound);
        StartCoroutine("WaitToHide", showTime);
    }

    public void HideSprite()
    {
        sprite.SetActive(false);
    }

    IEnumerator WaitToHide(float value)
    {
        yield return new WaitForSeconds(value);
        HideSprite();
        yield return null;
    }

    
}
