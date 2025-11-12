using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3FloorGroup : MonoBehaviour
{
    [field: SerializeField] public List<Level3Platform> Platforms = new List<Level3Platform>();

    [field: SerializeField] public bool CanBeModified = true;

    [SerializeField] private int minNumberOfIntactPlatforms = 3;

    private void Awake()
    {
        CanBeModified = true;
        Platforms.AddRange(GetComponentsInChildren<Level3Platform>());
    }

    public Level3Platform GetRandomPlatform()
    {
        int random = Random.Range(0, Platforms.Count);

        return Platforms[random];
    }

    [ContextMenu("TestRandomBurn")]
    public void BurnRandomPlatform()
    {
        if (!CanBeModified) return;

        Level3Platform platform = GetRandomPlatform();

        Platforms.Remove(platform);

        platform.SetPlatformAflame();

        PlatformFloorCheck();
    }

    public void BurnPlatform(Level3Platform platform)
    {
        if (!CanBeModified) return;

        Platforms.Remove(platform);

        platform.SetPlatformAflame();

        PlatformFloorCheck();
    }

    [ContextMenu("TestRandomSerpent")]
    public void SerpentRandomPlatform()
    {
        if (!CanBeModified) return;

        Level3Platform platform = GetRandomPlatform();

        Platforms.Remove(platform);

        platform.SerpentTailAttack();

        PlatformFloorCheck();
    }

    public void SerpentPlatform(Level3Platform platform)
    {
        if (!CanBeModified) return;

        Platforms.Remove(platform);

        platform.SerpentTailAttack();

        PlatformFloorCheck();
    }

    private void PlatformFloorCheck()
    {
        if (Platforms.Count <= minNumberOfIntactPlatforms) CanBeModified = false;
    }
}
