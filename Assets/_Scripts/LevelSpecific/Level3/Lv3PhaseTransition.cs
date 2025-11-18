using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3PhaseTransition : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject cameraObject;
    [SerializeField] protected GameObject bossHealthBar;

    [Header("Phase 1 elements")]
    [SerializeField] private CameraScrollUp cameraMovement;
    [SerializeField] private Lv3FloorsManager floorsManager;
    [SerializeField] private Phase1MiguelController phase1MiguelController;

    [Header("Transform Objects")]
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform cameraNewPosition;
    [SerializeField] private Transform bossSpawn;

    [Header("Prefabs")]
    [SerializeField] private GameObject phase2Boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(phase1MiguelController.gameObject);
        Destroy(floorsManager.gameObject);

        ConfigureCamera();
        ConfigurePlayer();
        ConfigureBoss();
    }

    private void ConfigureCamera()
    {
        cameraMovement.ShouldMove = false;
        cameraMovement.enabled = false;

        cameraObject.transform.position = cameraNewPosition.position;
    }

    private void ConfigurePlayer()
    {
        PlayerController controller = GameManager.Instance.PlayerInstance;
        controller.StunPlayer(2);
        controller.transform.position = playerSpawn.position;
    }

    private void ConfigureBoss()
    {
        bossHealthBar.SetActive(true);
        
        //Instantiate new Boss
    }
}
