using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newShakeProfile", menuName = "Data/ScreenShake/Profile")]
public class ScreenShakeProfile : ScriptableObject
{
    [field: Header("Impulse Source Settings")]
    [field: SerializeField] public float ImpactTime { get; private set; } = 0.2f;
    [field: SerializeField] public float ImpactForce { get; private set; } = 1f;
    [field: SerializeField] public Vector3 DefaultVelocity { get; private set; }
    [field: SerializeField] public AnimationCurve ImpulseCurve { get; private set; }

    [field : Header ("Impulse Listener Settings")]
    [field: SerializeField] public float ListenerAmplitude { get; private set; } = 1f;
    [field: SerializeField] public float ListenerFrequency { get; private set; } = 1f;
    [field: SerializeField] public float ListenerDuration { get; private set; } = 1f;
}
