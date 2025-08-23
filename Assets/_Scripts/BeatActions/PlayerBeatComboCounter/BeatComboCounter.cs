using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatComboCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterTextBox;
    [SerializeField] private Image timerImage;
    [SerializeField] private StyleRank[] styleRanks;

    public StyleRank currentRank { get; private set; }

    private int beatComboCounter;
    private int currentRankIndex = 0;
    private Core core;
    private Core_Health health;

    private float timeUntilDecay;
    private float rankDuration;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        health = core.GetCoreComponent<Core_Health>();
    }

    private void OnEnable()
    {
        health.OnDamageReceived += OnTimerDecay;
        BeatManager.Instance.OnWrongBeat += OnTimerDecay;
    }

    private void OnDisable()
    {
        health.OnDamageReceived -= OnTimerDecay;
        BeatManager.Instance.OnWrongBeat -= OnTimerDecay;
    }

    private void Start()
    {
        beatComboCounter = 0;
        currentRankIndex = 0;
        currentRank = styleRanks[0];

        if (timerImage != null)
        {
            timerImage.fillAmount = 1f;
            timerImage.color = currentRank.rankColor;
        }
        if (counterTextBox != null) counterTextBox.text = beatComboCounter.ToString();

        ResetDecayTimer();
    }

    private void Update()
    {
        ProgressTimer();
        if (timerImage != null) timerImage.fillAmount = Mathf.Clamp01( timeUntilDecay / rankDuration);
    }

    [ContextMenu("Test Combo Increase")]
    public void IncreaseComboCounter()
    {
        beatComboCounter++;

        if (counterTextBox != null) counterTextBox.text = beatComboCounter.ToString();

        if (currentRankIndex < styleRanks.Length - 1 && beatComboCounter >= styleRanks[currentRankIndex+1].rankThreshold)
        {
            currentRankIndex++;
            currentRank = styleRanks[currentRankIndex];

            //Debug.Log(currentRank.rankName);
        }

        ResetDecayTimer();
    }

    public void ResetDecayTimer()
    {
        timeUntilDecay = GetRankDuration(currentRank);
        if (timerImage != null)
        {
            timerImage.fillAmount = 1f;
            timerImage.color = currentRank.rankColor;
        }
    }

    public float GetRankDuration(StyleRank rank)
    {
        float durationinBeats = rank.beatsForRankDecay;
        float beatDuration = 60f / BeatManager.Instance.BPM;

        rankDuration = durationinBeats * beatDuration;

        return rankDuration;
    }

    [ContextMenu("Test Timer Decay")]
    public void OnTimerDecay()
    {
        if (currentRankIndex > 0)
        {
            currentRankIndex--;
            currentRank = styleRanks[currentRankIndex];
        }

        beatComboCounter = currentRank.rankThreshold;
        if (counterTextBox != null) counterTextBox.text = beatComboCounter.ToString();

        //Debug.Log(currentRank.rankName);

        ResetDecayTimer();
    }

    private void ProgressTimer()
    {
        timeUntilDecay -= Time.deltaTime;

        if (timeUntilDecay <= 0) OnTimerDecay();
    }
}
