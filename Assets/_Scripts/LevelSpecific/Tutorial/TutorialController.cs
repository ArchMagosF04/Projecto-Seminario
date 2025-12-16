using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCam;

    [SerializeField] private Transform[] camerasRoomTargets;
    [SerializeField] private Transform[] playerTeleportPositions;

    [Header("Ortho Sizes")]
    [SerializeField] private float closeCamOrtho = 2.27f;
    [SerializeField] private float normalCamOrtho = 6.569685f;

    private void Awake()
    {
        cinemachineCam.Follow = camerasRoomTargets[0];
        cinemachineCam.m_Lens.OrthographicSize = closeCamOrtho;
    }

    public void ChangeCurrentRoom(int index)
    {
        if (index >= camerasRoomTargets.Length)
        {
            Debug.LogError("Out Of array cam", this);
            return;
        }

        if (index > 0) cinemachineCam.m_Lens.OrthographicSize = normalCamOrtho;

        cinemachineCam.Follow = camerasRoomTargets[index];

        GameManager.Instance.PlayerInstance.transform.position = playerTeleportPositions[index].position;
    }
}
