using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3FloorsManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private LayerMask platformMask;

    [Header("PlatformDetector")]
    [SerializeField] private Vector3 boxDetectionSize;
    [SerializeField] private Vector3 boxDetectionOffset;
    [SerializeField] private bool drawBox;

    //Variables
    public List<Level3FloorGroup> AvailableFloors = new List<Level3FloorGroup>();
    public List<Level3FloorGroup> FloorsOnScreen = new List<Level3FloorGroup>();

    public void UpdateAvailableFloors()
    {
        AvailableFloors.Clear();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + boxDetectionOffset, boxDetectionSize, 0, platformMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Level3FloorGroup>(out Level3FloorGroup floor))
            {
                if (floor.CanBeModified)
                {
                    AvailableFloors.Add(floor);
                }
            }
        }
    }

    public Level3Platform GetPlatform()
    {
        if (AvailableFloors.Count <= 0) return null;

        int rand = Random.Range(0, AvailableFloors.Count);

        return AvailableFloors[rand].GetRandomPlatform();
    }

    public void SerpentAttack()
    {
        if (AvailableFloors.Count <= 0) return;

        int rand = Random.Range(0, AvailableFloors.Count);

        AvailableFloors[rand].SerpentRandomPlatform();
    }

    public void FlameAttack()
    {
        if (AvailableFloors.Count <= 0) return;

        int rand = Random.Range(0, AvailableFloors.Count);

        AvailableFloors[rand].BurnRandomPlatform();
    }

    private void FixedUpdate()
    {
        UpdateAvailableFloors();

        //Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + boxDetectionOffset, boxDetectionSize, 0, platformMask);

        //foreach (Collider2D collider in colliders)
        //{
        //    if (collider.TryGetComponent<Level3FloorGroup>(out Level3FloorGroup floor))
        //    {
        //        FloorsOnScreen.Add(floor);
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        if (drawBox)
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireCube(transform.position + boxDetectionOffset, boxDetectionSize);
        }
    }
}
