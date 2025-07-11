using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss1Technique<T> : State<T>
{
    private BossMovement movementController;
    private Transform stageLocation;
    private GameObject boss;
    private GameObject camera;
    //private GameObject atkProjectiles;
    private GameObject player;
    private GameObject anchor;
    private GameObject plataform1;
    private GameObject plataform2;
    private EnemyStats enemyInfo;
    private float techniqueDelayTime;
    private float currenttechniqueDelayTime = 0;
    private bool atCenterStage;
    public bool AtCenterStage { get { return atCenterStage; } }
    bool plataformsActivated = false;

    
    //private float wavesSpawnInterval;
    //private float wavesIntervalTimer;
    //private float wavesMaxSize;
    //private float wavesGrowthSpeed;

    private BeatDetector beatDetector;

    private Animator anim;

    public Boss1Technique(BossMovement bossMovement, Transform targetLocation, GameObject user, GameObject plataform1, GameObject plataform2, EnemyStats enemyInformation, BeatDetector beatDetector, GameObject virtualCamera, GameObject player, GameObject playerAnchor, Animator anim)
    {
        movementController = bossMovement;
        stageLocation = targetLocation;
        boss = user;
        this.plataform1 = plataform1;
        this.plataform2 = plataform2;
        enemyInfo = enemyInformation;
        camera = virtualCamera;
        techniqueDelayTime = enemyInformation.TechniqueDelayTime;
        //wavesSpawnInterval = enemyInformation.WavesSpawnInterval;
        //atkProjectiles = enemyInformation.GetProyectile(1);
        //wavesMaxSize = enemyInformation.WavesMaxSize;
        //wavesGrowthSpeed = enemyInformation.WavesGrowthSpeed;
        this.beatDetector = beatDetector;
        this.player = player;
        anchor = playerAnchor;
        this.anim = anim;
    }

    public override void Enter()
    {
        player.transform.SetParent(anchor.transform);
        anim.SetTrigger("SecretAttack");
    }

    public override void FixedExecute()
    {
        if (!atCenterStage)
        {
            movementController.Move(enemyInfo.Acceleration, new Vector2(FindCenterStage(), 0));
        }
    }

    public override void Execute()
    {
        if (Mathf.Abs(boss.transform.position.x - stageLocation.position.x) < 0.7f)
        {
            if (currenttechniqueDelayTime < techniqueDelayTime)
            {
                currenttechniqueDelayTime += Time.deltaTime;
            }
            else
            {
                atCenterStage = true;
                currenttechniqueDelayTime = 0;
            }
        }

        if (atCenterStage && plataformsActivated == false)
        {
            ActivatePlataforms();
        }
    }

    public override void Exit()
    {
        atCenterStage = false;
        anim.SetTrigger("Idle");
        DeactivatePlataforms();
    }

    private float FindCenterStage()
    {
        Vector3 result = stageLocation.position - boss.transform.position;
        return result.normalized.x;
    }

    private void ActivatePlataforms()
    {
        plataform1.GetComponent<MovingPlataform>().Shake(true, beatDetector);
        plataform2.GetComponent<MovingPlataform>().Shake(true, beatDetector);
        camera.gameObject.GetComponent<ShakeToRythm>().ActivateShake(true, beatDetector);
        anchor.gameObject.GetComponent<ShakeToSides>().Shake(true, beatDetector);
        plataformsActivated = true;
    }

    private void DeactivatePlataforms()
    {
        plataform1.GetComponent<MovingPlataform>().Shake(false, beatDetector);
        plataform2.GetComponent<MovingPlataform>().Shake(false, beatDetector);
        camera.gameObject.GetComponent<ShakeToRythm>().ActivateShake(false, beatDetector);
        anchor.gameObject.GetComponent<ShakeToSides>().Shake(false, beatDetector);
        player.transform.SetParent(null);
        plataformsActivated = false;
    }
}
