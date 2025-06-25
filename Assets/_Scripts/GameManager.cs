using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private static List<GameObject> WeaponPrefabs;
    [SerializeField] private static int availableWeapons=1;
    [SerializeField] int maxLevels;
    [SerializeField] int UnlockedLevels = 1;

    public int weaponSelected;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockNextLevel()
    {
        if(UnlockedLevels < maxLevels)
        {
            UnlockedLevels += 1;
        }
    }

    public void UnlockWeapon(PlayerWeapon weapon)
    {
        availableWeapons += 1;
    }

    public static int GetAvailableWeapons()
    {
        return availableWeapons;
    }

    public static void EquipWeapon(int weaponIndex)
    {
        // PlayerWeapon
    }
}
