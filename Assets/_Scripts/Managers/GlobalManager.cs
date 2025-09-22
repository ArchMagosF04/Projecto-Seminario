using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    private int currentSelectedWeapon;
    public int selectedWeapon { get { return currentSelectedWeapon; } }

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

    public void SetCurrentWeapon(int currentWeapon)
    {
        currentSelectedWeapon = currentWeapon;
    }
}
