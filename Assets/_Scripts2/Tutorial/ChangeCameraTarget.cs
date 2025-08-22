using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeCameraTarget : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject newTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        virtualCamera.Follow = newTarget.transform;
    }
}
