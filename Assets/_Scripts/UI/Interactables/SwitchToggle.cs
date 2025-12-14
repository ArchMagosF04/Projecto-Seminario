using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider Setup")]
    [SerializeField, Range(0, 1f)] private float sliderValue;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Image handleImage;

    public bool CurrentValue { get; private set; }

    private Slider slider;

    [Header("Animation")]
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private bool usesUnscaledDeltaTime;

    private Coroutine animationCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    private ToggleSwitchGroupManager toggleSwitchGroupManager;

    private bool previousValue;

    private Color deselectColor;

    protected void OnValidate()
    {
        SetupToggleComponents();

        slider.value = sliderValue;
    }

    protected void SetupToggleComponents()
    {
        if (slider != null) return;

        SetUpSliderComponent();
    }

    protected void SetUpSliderComponent()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("No slider found", this);
        }

        slider.interactable = false;
        var sliderColors = slider.colors;
        sliderColors.disabledColor = Color.white;
        slider.colors = sliderColors;
    }

    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        toggleSwitchGroupManager = manager;
    }

    private void Awake()
    {
        SetupToggleComponents();
        deselectColor = handleImage.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public void Toggle()
    {
        if (toggleSwitchGroupManager != null)
            toggleSwitchGroupManager.ToggleGroup(this);
        else
            SetStateAndStartAnimation(!CurrentValue);
    }

    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        previousValue = CurrentValue;
        CurrentValue = state;

        if (previousValue != CurrentValue)
        {
            if (CurrentValue)
            {
                onToggleOn?.Invoke();
            }
            else
            {
                onToggleOff?.Invoke();
            }
        }

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += usesUnscaledDeltaTime? Time.unscaledDeltaTime : Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                yield return null;
            }
        }

        slider.value = endValue;
    }

    public void OnToggleSelected()
    {
        handleImage.color = selectedColor;
    }

    public void OnToggleDeselected()
    {
        handleImage.color = deselectColor;
    }
}
