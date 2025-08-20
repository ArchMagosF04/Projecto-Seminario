using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimatorEvent : MonoBehaviour
{
    public Action OnAnimationFinishedTrigger;

    private void OnAnimationFinished()
    {
        OnAnimationFinishedTrigger?.Invoke();
    }
}
