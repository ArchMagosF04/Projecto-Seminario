using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatComboCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterTextBox;
    [SerializeField] private Image rankIndicatorImage;
    [SerializeField] private SquashAndStretch comboIncreaseImage;
    [SerializeField] private StyleRank[] styleRanks;

    public StyleRank currentRank { get; private set; }

    private int beatComboCounter;
    private int currentRankIndex = 0;
    public int MaxCombo { get; private set; }
    private Core core;
    private Core_Health health;

    private float timeUntilDecay;
    private float rankDuration;

    private Color workColor;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        health = core.GetCoreComponent<Core_Health>();

        //foreach (StyleRank rank in styleRanks)
        //{
        //    Debug.Log(rank.rankDamageMultiplier);
        //}
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
        MaxCombo = 0;
        beatComboCounter = 0;
        currentRankIndex = 0;
        currentRank = styleRanks[0];

        if (rankIndicatorImage != null)
        {
            workColor = currentRank.rankColor;
            workColor.a = 1;
            rankIndicatorImage.color = workColor;
        }
        if (counterTextBox != null) counterTextBox.text = beatComboCounter.ToString();

        ResetDecayTimer();
    }

    private void Update()
    {
        ProgressTimer();
        if (rankIndicatorImage != null)
        {
            workColor = rankIndicatorImage.color;
            workColor.a = timeUntilDecay / rankDuration;
            rankIndicatorImage.color = workColor;
        }
    }

    [ContextMenu("Test Combo Increase")]
    public void IncreaseComboCounter(int amount)
    {
        beatComboCounter += amount;
        comboIncreaseImage.PlaySquashAndStretch();
        if(MaxCombo < beatComboCounter) { MaxCombo = beatComboCounter; }
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
        if (rankIndicatorImage != null)
        {
            workColor = currentRank.rankColor;
            workColor.a = 1;
            rankIndicatorImage.color = workColor;
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

    public float GetDamageMultiplier()
    {
        return currentRank.rankDamageMultiplier;
    }
}
