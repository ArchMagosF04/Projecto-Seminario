using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushPlatform : MonoBehaviour
{
    [Header("Activation Time")]
    [SerializeField] private int beatsTillPush = 10;
    [SerializeField] private int beatsTillPushP2 = 7;

    [Header("Knockback Settings")]
    [SerializeField] private float pushStrength = 20f;
    [SerializeField] private Vector2 pushAngle;
    [SerializeField, Range(-1, 1)] private int direction;

    [Header("UI Telegraph")]
    [SerializeField] private Image timerImage;

    [Header("Debug Variables")]
    [SerializeField] private Collider2D player;

    [SerializeField] private int beatTimer;

    private int beatsUntilActivation;

    private void Start()
    {
        beatsUntilActivation = beatsTillPush;
        beatTimer = beatsUntilActivation;
        timerImage.fillAmount = 1;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatCounter;
    }

    public void BeatCounter()
    {
        beatTimer--;

        timerImage.fillAmount = (float)beatTimer / (float)beatsUntilActivation;

        if (beatTimer == 0)
        {
            PushPlayer();
        }
    }

    public void PushPlayer()
    {
        beatTimer = beatsUntilActivation;
        timerImage.fillAmount = 1;

        if (player == null) return;

        if(player.TryGetComponent<Core_Knockback>(out Core_Knockback knockback))
        {
            knockback.Knockback(pushAngle, pushStrength, direction);
        }
    }

    public void EnterSecondPhase()
    {
        beatsUntilActivation = beatsTillPushP2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision;

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatCounter;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
        beatTimer = beatsUntilActivation;
        timerImage.fillAmount = 1;

        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatCounter;
    }
}
