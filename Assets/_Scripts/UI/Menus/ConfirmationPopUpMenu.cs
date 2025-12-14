using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationPopUpMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
    {
        this.gameObject.SetActive(true);

        this.displayText.text = displayText;

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });

        cancelButton.Select();
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
