using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public PlayerController PlayerInstance {  get; private set; }

    [field: SerializeField] public bool IsGameActive { get; private set; }

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

    public void ToggleGameActiveState(bool state) => IsGameActive = state;
}
