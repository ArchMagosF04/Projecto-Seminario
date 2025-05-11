using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimatorEvent : MonoBehaviour
{
    public UnityEvent OnAnimationTrigger;

    private void AnimationTrigger()
    {
        OnAnimationTrigger?.Invoke();
    }
}
