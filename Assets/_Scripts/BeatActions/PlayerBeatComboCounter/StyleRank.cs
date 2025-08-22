using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StyleRank
{
    public string rankName;
    public int rankThreshold;
    [Range(1f, 2f)] public float rankDamageMultiplier;
    [Range(1f, 10f)] public float beatsForRankDecay;
    public Color rankColor;
}
