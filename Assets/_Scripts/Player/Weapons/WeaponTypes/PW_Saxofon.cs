using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PW_Saxofon : PlayerWeapon
{
    [Header("Weapon Stats")]

    [Tooltip("Daño de cada proyectil")]
    [SerializeField] protected float basicAttackDamage = 3f;

    [Tooltip("Duracion del effecto de estado en segundos")]
    [SerializeField] private float statusDuration = 3;

    //[Tooltip("Cual es la frecuencia de beat que este arma reconoce como correcto")]
    //[SerializeField] private BeatEnumarator designatedBeat = 0;

    [Tooltip("Cuanto mana recargara cada impacto de proyectil")]
    [SerializeField] protected float manaOnBeatHit;

    [SerializeField] protected SoundLibraryObject soundLibrary;

    [Tooltip("Lista de prefabs de proyectiles del arma. El arma utiliza un random para elegir uno de estos proyectiles cada vez que dispara")]
    [SerializeField] private GameObject[] projectiles;

    protected override void Awake()
    {
        base.Awake();
        soundLibrary.Initialize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.OnAttackAction += BasicAttackDamage;
        OnSpecialEnter += SpecialAttackDamage;        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.OnAttackAction -= BasicAttackDamage;
        OnSpecialEnter -= SpecialAttackDamage;        
    }   

    private void BasicAttackDamage()
    {
            int randomSound = Random.Range(0, 1);

            if (isOnBeat)
            {
                Shoot(0, basicAttackDamage, true, false);
            }
            else
            {
                Shoot(0, basicAttackDamage, false, false);
            }

            if (isOnBeat)
            {
                float multiplier = 0.8f;
                multiplier = beatCombo.currentRank.rankDamageMultiplier;
                beatCombo.IncreaseComboCounter();
                StartCoroutine(BumpUpMusic());                
            }
            else
            {
                Shoot(0, basicAttackDamage, false, false);
            }         

    }

    private void SpecialAttackDamage()
    {
        Shoot(1, basicAttackDamage, isOnBeat, true);

        //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("Special")).Play();

        if (isOnBeat)
        {
            float multiplier = 0.8f;
            multiplier = beatCombo.currentRank.rankDamageMultiplier;
            beatCombo.IncreaseComboCounter();
        } 
    }

    IEnumerator BumpUpMusic()
    {
        BeatManager.Instance.IncreaseMusicVolume(1);

        yield return new WaitForSecondsRealtime(1);

        BeatManager.Instance.DecreaseMusicVolume(0.7f);

        yield break;
    }

    private void RecoverMana()
    {
        manaComponent.IncreaseMana(manaOnBeatHit);
        float multiplier = 0.8f;
        multiplier = beatCombo.currentRank.rankDamageMultiplier;
        beatCombo.IncreaseComboCounter();
    }

    private void Shoot(int projectileNumber, float damage, bool onBeat, bool special, float mod = 1)
     {
        if (!special)
        {
            GameObject projectile = null;
            GameObject projectile2 = null;

            projectile = GameObject.Instantiate(projectiles[projectileNumber], transform.position, transform.rotation);
            projectile2 = GameObject.Instantiate(projectiles[projectileNumber], transform.position, transform.rotation);

            projectile.GetComponent<SaxofonProjectile>().SetDamage(damage * mod);
            projectile2.GetComponent<SaxofonProjectile>().SetDamage(damage * mod);

            if (onBeat) projectile.GetComponent<SaxofonProjectile>().OnHit += RecoverMana;

            if (onBeat)
            {
                SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnBeatHit-" + projectileNumber.ToString())).Play();
                Debug.Log("OnBeatHit-" + projectileNumber);
            }
            else
            {
                projectile.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                projectile2.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnMissHit-" + projectileNumber.ToString())).Play();
                Debug.Log("OnMissHit-" + projectileNumber);
            }

            projectile.GetComponent<SaxofonProjectile>().LaunchProjectile(transform.right + transform.up);
            projectile2.GetComponent<SaxofonProjectile>().LaunchProjectile(transform.right - transform.up);
        }
        else
        {
            GameObject projectile = null;            

            projectile = GameObject.Instantiate(projectiles[projectileNumber], transform.position, transform.rotation);
            

            projectile.GetComponent<SaxofonSpecialProjectile>().SetDamage(damage * mod);

            if (onBeat) projectile.GetComponent<SaxofonSpecialProjectile>().OnHit += RecoverMana;

            if (onBeat)
            {
                SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnBeatHit-" + projectileNumber.ToString())).Play();
                Debug.Log("OnBeatHit-" + projectileNumber);
            }
            else
            {
                //projectile.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnMissHit-" + projectileNumber.ToString())).Play();
                Debug.Log("OnMissHit-" + projectileNumber);
            }

            projectile.GetComponent<SaxofonSpecialProjectile>().LaunchProjectile(transform.right);
            return;
        }
    }
}
