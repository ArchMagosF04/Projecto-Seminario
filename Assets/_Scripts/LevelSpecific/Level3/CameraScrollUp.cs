using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollUp : MonoBehaviour
{
    [SerializeField] private float speed;

    public bool ShouldMove;

    private void Update()
    {
        if (ShouldMove)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
    }
}
