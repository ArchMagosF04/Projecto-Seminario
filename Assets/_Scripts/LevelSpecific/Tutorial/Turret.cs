using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Projectile bullet;
    [SerializeField] private SoundID shootSound;

    private void OnEnable()
    {
        BeatManager.Instance.intervals[5].OnBeatEvent += Shoot;
    }

    private void OnDisable()
    {
        BeatManager.Instance.intervals[5].OnBeatEvent -= Shoot;
    }

    private void Shoot()
    {
        Projectile newBullet = Instantiate(bullet, transform.position + new Vector3(0, 0.5f), Quaternion.identity);

        if (shootSound.IsValid()) BroAudio.Play(shootSound);
        newBullet.LaunchProjectile(Vector2.up);
    }
}
