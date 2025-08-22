using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measurer : MonoBehaviour
{
    [Header("Distance Radius")]
    [SerializeField] private Color distanceColor = Color.white;
    [SerializeField] private bool showDistance;
    [SerializeField] private float distanceToMeasure;

    [Header("Angle Measure")]
    [SerializeField] private Color angleColor = Color.white;
    [SerializeField] private bool showAngle;
    [SerializeField, Range(0, 360)] private float angleToMeasure;

    private void OnDrawGizmos()
    {
        if (showDistance)
        {
            Gizmos.color = distanceColor;

            Gizmos.DrawWireSphere(transform.position, distanceToMeasure);
        }
        if (showAngle)
        {
            Gizmos.color = angleColor;

            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, angleToMeasure / 2) * transform.up * distanceToMeasure);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -angleToMeasure / 2) * transform.up * distanceToMeasure);
        }
    }
}
