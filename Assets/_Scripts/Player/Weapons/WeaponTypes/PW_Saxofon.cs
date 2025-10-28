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

    [Tooltip("Duracion del modo especial en segundos")]
    [SerializeField] private float specialModeDuration = 3;
    private float currentSpecialDuration;

    [Tooltip("Aumento de daño base a cada proyectil durante el modo especial. Formula: (basicAttackDamage + specialModeDamageBomus) * damageMultiplier")]
    [SerializeField] private float specialModeDamageBonus;  

    [Tooltip("Cual es la frecuencia de beat que este arma reconoce como correcto")]
    [SerializeField] private BeatEnumarator designatedBeat = 0;

    [Tooltip("Cuanto mana recargara cada impacto de proyectil")]
    [SerializeField] protected float manaOnBeatHit;

    [SerializeField] protected SoundLibraryObject soundLibrary;

    [Tooltip("Lista de prefabs de proyectiles del arma. El arma utiliza un random para elegir uno de estos proyectiles cada vez que dispara")]
    [SerializeField] private GameObject[] projectiles;

    //[SerializeField] private Animator animator2;

    private bool specialMode;

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
                Shoot(randomSound, basicAttackDamage, true, specialMode);
            }
            else
            {
                Shoot(randomSound, basicAttackDamage, false, specialMode);
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
                Shoot(randomSound, basicAttackDamage, false, false);
            }         

    }

    private void SpecialAttackDamage()
    {
        if (specialMode == false)
        {
            specialMode = true;
            //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("Special")).Play();
            specialMode = true;
            currentSpecialDuration = specialModeDuration;
            anim.SetBool("Special2", true);
        }
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

    private void Shoot(int projectileNumber, float damage, bool onBeat, bool special,float mod = 1)
     {
        GameObject projectile = null;
        GameObject projectile2 = null;

        projectile = GameObject.Instantiate(projectiles[projectileNumber], transform.position, transform.rotation);
        projectile2 = GameObject.Instantiate(projectiles[projectileNumber], transform.position, transform.rotation);

        projectile.GetComponent<Projectile>().SetDamage(damage * mod);
        projectile2.GetComponent<Projectile>().SetDamage(damage * mod);

        if (onBeat) projectile.GetComponent<Projectile>().OnHit += RecoverMana;

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
        
        projectile.GetComponent<Projectile>().LaunchProjectile(transform.right + transform.up);
        projectile2.GetComponent<Projectile>().LaunchProjectile(transform.right - transform.up);

        //StopAllCoroutines();

        //currentFireCooldown = fireRate;
    }
}
