using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderInteraction : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private bool showSliderValue = true;
    [SerializeField, Range(0, 2)] private int decimalPointsToShow;
    [SerializeField] private int multiplyValueShown = 1;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        if (!showSliderValue) valueText?.gameObject.SetActive(false);
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChange);

        //if (showSliderValue)
        //{
        //    valueText.SetText(slider.value.ToString("F" + decimalPointsToShow));
        //}
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(HandleSliderValueChange);
    }

    public void HandleSliderValueChange(float value)
    {
        if (!showSliderValue) return;

        value *= multiplyValueShown;

        valueText.SetText(value.ToString("F"+decimalPointsToShow));
    }
}
