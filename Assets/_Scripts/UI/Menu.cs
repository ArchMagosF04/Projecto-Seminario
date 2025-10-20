using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [field: SerializeField] public string MenuName { get; private set; }
    [field: SerializeField] public bool IsOpen { get; private set; }
    [field: SerializeField] public Menu previousMenu { get; private set; }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        IsOpen = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
