using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3PlatformManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private LayerMask platformMask;

    [Header("PlatformDetector")]
    [SerializeField] private Vector3 boxDetectionSize;
    [SerializeField] private Vector3 boxDetectionOffset;
    [SerializeField] private bool drawBox;

    //Variables
    [SerializeField] private List<Level3Platform> deactivatedPlatforms;
    [SerializeField] private List<Level3Platform> activePlatforms;
    public List<Level3Platform> activePlatformsOnScreen;

    private void Start()
    {
        deactivatedPlatforms = new List<Level3Platform>();
        activePlatforms = new List<Level3Platform>();
    }

    private void OnEnable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent += ActivatePlatform;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= ActivatePlatform;
    }

    private void ActivatePlatform()
    {
        SearchPlatforms();

        if (deactivatedPlatforms.Count <= 0) return;

        int random = Random.Range(0, deactivatedPlatforms.Count);

        Level3Platform chosenPlatform = deactivatedPlatforms[random];

        chosenPlatform.TogglePlatform(true);

        deactivatedPlatforms.RemoveAt(random);
        activePlatforms.Add(chosenPlatform);
    }

    private void SearchPlatforms()
    {
        deactivatedPlatforms.Clear();
        activePlatformsOnScreen.Clear();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + boxDetectionOffset, boxDetectionSize, 0, platformMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Level3Platform>(out Level3Platform platform))
            {
                if (platform.isPlatformActive)
                {
                    if (!platform.wasPlatformModified) activePlatformsOnScreen.Add(platform);

                    continue;
                }

                if (!deactivatedPlatforms.Contains(platform)) deactivatedPlatforms.Add(platform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawBox)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireCube(transform.position + boxDetectionOffset, boxDetectionSize);
        }
    }
}
