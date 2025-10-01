using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHelper : MonoBehaviour
{
    private GlobalManager gManager;
    [SerializeField] LevelSelectorController levelSelector;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GlobalManager.Instance;
    }

    public void ChangeWeapon(int weapon)
    {
        gManager.SetCurrentWeapon(weapon);
    }

    public void SetLevel()
    {
        gManager.SetCurrentLevel(levelSelector.GetSelectedLevel(), levelSelector.GetSelectedLevelIdex());
    }

}
