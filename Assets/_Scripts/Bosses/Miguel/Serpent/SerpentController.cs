using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SerpentController : MonoBehaviour
{
    [SerializeField] private Bullet_DamageLess[] smallTornado;
    [SerializeField] private Bullet_DamageLess[] largeTornado;

    [SerializeField] private Transform tornadoSpawnPoint;

    private int beatTimer;
    private int attackLength;

    public enum SerpentState { Idle, SmallAttack, LargeAttack }

    public SerpentState CurrentState;

    private int lastSmallTornadoIndex = 0;
    private int lastLargeTornadoIndex = 0;
    private int sameSmallTornadoCounter;
    private int sameLargeTornadoCounter;

    private bool skipNextBeat = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;
    }

    public void LargeTornadoAttack(int length)
    {
        if (CurrentState != SerpentState.Idle) return;

        attackLength = length;
        beatTimer = 0;
        CurrentState = SerpentState.LargeAttack;

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatTimer;
    }

    public void SmallTornadoAttack(int length)
    {
        if (CurrentState != SerpentState.Idle) return;

        attackLength = length;
        beatTimer = 0;
        CurrentState = SerpentState.SmallAttack;

        BeatManager.Instance.intervals[0].OnBeatEvent += BeatTimer;
    }

    private void BeatTimer()
    {
        if (beatTimer >= attackLength)
        {
            beatTimer = 0;
            skipNextBeat = false;
            CurrentState = SerpentState.Idle;
            BeatManager.Instance.intervals[0].OnBeatEvent -= BeatTimer;

            return;
        }

        if (skipNextBeat)
        {
            skipNextBeat = false;
            return;
        }

        animator.SetTrigger("Attack");

        if (CurrentState == SerpentState.LargeAttack)
        {
            SpawnLargeTonado();
        }
        else if (CurrentState == SerpentState.SmallAttack)
        {
            SpawnSmallTornado();
        }

        beatTimer++;
        skipNextBeat = true;
    }

    private void SpawnSmallTornado()
    {
        int randomIndex = 0;

        randomIndex = Random.Range(0, smallTornado.Length);

        if (lastSmallTornadoIndex == randomIndex)
        {
            sameSmallTornadoCounter++;

            if (sameSmallTornadoCounter >= 2)
            {
                switch(randomIndex)
                {
                    case 0:
                        randomIndex = 1;
                        break;
                    case 1:
                        randomIndex = 2;
                        break;
                    case 2:
                        randomIndex = 0;
                        break;
                }
                sameLargeTornadoCounter = 0;
            }
        }

        Bullet_DamageLess newTornado = Instantiate(smallTornado[randomIndex], tornadoSpawnPoint.position, Quaternion.identity);
        newTornado.LaunchProjectile(Vector2.left);

        lastSmallTornadoIndex = randomIndex;
    }

    private void SpawnLargeTonado()
    {
        int randomIndex = 0;

        randomIndex = Random.Range(0, largeTornado.Length);

        if (lastLargeTornadoIndex == randomIndex)
        {
            sameLargeTornadoCounter++;

            if (sameLargeTornadoCounter >= 2)
            {
                if (randomIndex == 0) randomIndex = 1;
                else randomIndex = 0;
                sameLargeTornadoCounter = 0;
            }
        }

        Bullet_DamageLess newTornado = Instantiate(largeTornado[randomIndex], tornadoSpawnPoint.position, Quaternion.identity);
        newTornado.LaunchProjectile(Vector2.left);

        lastLargeTornadoIndex = randomIndex;
    }
}
