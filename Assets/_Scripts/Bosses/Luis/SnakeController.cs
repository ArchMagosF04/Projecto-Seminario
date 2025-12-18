using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private int beatTimer;
    private int attackLength;

    [SerializeField] private Transform highSpawnPoint;
    [SerializeField] private Transform lowSpawnPoint;

    [SerializeField] private Projectile highProjectile;
    [SerializeField] private Projectile lowProjectile;

    [SerializeField] private SoundID attackSound;

    public enum SerpentState { Idle, AttackHigh, AttackLow, Death }

    public SerpentState CurrentState;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
    }

    public void ShootHighProjectile()
    {
        animator.SetTrigger("Attack");

        Projectile newBullet = Instantiate(highProjectile, highSpawnPoint.position, Quaternion.identity);

        if (attackSound.IsValid()) BroAudio.Play(attackSound);
        newBullet.LaunchProjectile(Vector2.left);
    }

    public void ShootLowProjectile()
    {
        animator.SetTrigger("Attack");

        Projectile newBullet = Instantiate(lowProjectile, lowSpawnPoint.position, Quaternion.identity);

        if (attackSound.IsValid()) BroAudio.Play(attackSound);
        newBullet.LaunchProjectile(Vector2.left);
    }

    public void KillSnake()
    {
        animator.SetBool("Death", true);
    }
}
