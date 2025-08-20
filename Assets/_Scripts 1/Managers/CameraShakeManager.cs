using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    [SerializeField] private float globalShakeForce = 1f;
    [SerializeField] private CinemachineImpulseListener impulseListener;

    private CinemachineImpulseDefinition impulseDefinition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    public void ScreenShakeFromProfile(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        SetUpScreenShakeSettings(profile, impulseSource);

        impulseSource.GenerateImpulseWithForce(profile.ImpactForce);
    }

    private void SetUpScreenShakeSettings(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        impulseDefinition.m_ImpulseDuration = profile.ImpactTime;
        impulseSource.m_DefaultVelocity = profile.DefaultVelocity;
        impulseDefinition.m_CustomImpulseShape = profile.ImpulseCurve;

        impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.ListenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = profile.ListenerFrequency;
        impulseListener.m_ReactionSettings.m_Duration = profile.ListenerDuration;
    }
}
