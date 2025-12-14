using AiryUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPage : MonoBehaviour
{
    [Header("Manual References")]
    [Tooltip("Defines the GameObject that will be selected by default when opeing this menu")]
    [SerializeField] private Selectable objectFirstSelected;
    [SerializeField] private MenuPage previousPage;

    [Header("Automatic References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private EventSystem eventSystem;

    public bool IsMenuOpen;

    private Coroutine currentCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent OnPageOpen;
    [SerializeField] private UnityEvent OnPageClose;

    private void Awake()
    {
        if (canvas == null) { canvas = GetComponent<Canvas>(); }
        eventSystem = FindFirstObjectByType<EventSystem>();
        CloseMenu();
    }

    [ContextMenu("Open Menu")]
    public void OpenMenu()
    {
        //if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        //currentCoroutine = StartCoroutine(OpenMenuCoroutine());
        ChangeObjectSelected();

        OnPageOpen?.Invoke();

        IsMenuOpen = true;
        canvas.enabled = true;

    }

    private IEnumerator OpenMenuCoroutine()
    {
        yield return new WaitForSeconds(0.25f);

        IsMenuOpen = true;
        canvas.enabled = true;

        ChangeObjectSelected();
    }

    [ContextMenu("Close Menu")]
    public void CloseMenu()
    {
        //if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        //currentCoroutine = StartCoroutine(CloseMenuRoutine());

        canvas.enabled = false;
        IsMenuOpen = false;

        OnPageClose?.Invoke();
    }

    private IEnumerator CloseMenuRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        canvas.enabled = false;
        IsMenuOpen = false;
    }

    public void ChangeObjectSelected()
    {
        if (eventSystem == null) Debug.Log("No event system reference", this);

        if (objectFirstSelected == null) Debug.Log("Object to jump to not selected", this);

        eventSystem.SetSelectedGameObject(objectFirstSelected.gameObject);
    }

    public MenuPage GoToPreviousPage()
    {
        if (IsMenuOpen && previousPage != null)
        {
            CloseMenu();
            previousPage.OpenMenu();

            return previousPage;
        }

        return null;
    }
}
