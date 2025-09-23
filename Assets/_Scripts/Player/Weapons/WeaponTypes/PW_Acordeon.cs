using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PW_Acordeon : PlayerWeapon
{
    [Header("Weapon Stats")]

    [Tooltip("Daño de cada proyectil")]
    [SerializeField] protected float basicAttackDamage = 3f;

    [Tooltip("Multiplicador de daño por cada nivel de carga cuando se mantiene apretado el boton de ataque. Formula: DañoBasico * (Multiplicador * Nivel de carga)")]
    [SerializeField] private float damageMultiplier = 1.5f;

    [Tooltip("Multiplicador de daño cuando se falla el ataque cargado. Formula: DañoBasico * penaltyMultiplier. \n Tabla de Referencia:\n 0.25= 75% menos daño\n 0.5 = 50% menos daño\n 0.75 = 25% menos daño\n 1 = AtaqueNormal sin reduccion")]
    [SerializeField] private float damagePenalty = 0.5f;

    [Tooltip("Por cuantos beats puede mantener el ataque cargado el jugador, antes de que la carga falle automaticamente y lanze un proyectil con daño penalizado")]
    [SerializeField] private int maxHoldTimeInBeats = 7;

    [Tooltip("Cual es la frecuencia de beat que este arma reconoce como correcto")]
    [SerializeField] private BeatEnumarator designatedBeat = 0;
    //[SerializeField] protected float specialAttackSpeed = 1f;

    [Tooltip("Cuanto mana recargara cada impacto de proyectil")]
    [SerializeField] protected float manaOnBeatHit;

    //[SerializeField] protected SoundLibraryObject soundLibrary;
    [Tooltip("Lista de prefabs de proyectiles del arma. El arma utiliza un random para elegir uno de estos proyectiles cada vez que dispara")]
    [SerializeField] private GameObject[] projectiles;
    


    private int beats;
    private int charge;
    private bool specialMode;

    protected override void Awake()
    {
        base.Awake();        
        //soundLibrary.Initialize();
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
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent -= SpecialHitOnBeat;
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent -= CountBeats;
        OnExit -= UnsubFromBeat;

    }

    private void Update()
    {
        if (beats>=7)
        {
            GameObject projectile = null;
            int randomSound = Random.Range(0, 3);
            GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * damagePenalty);
            projectile.GetComponent<Projectile>().LaunchProjectile(transform.right);
        }
    }

    private void BasicAttackDamage()
    {
        int randomSound = Random.Range(0, 3);

        if (isOnBeat)
        {
            BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent += CountBeats;
            float multiplier = 0.8f;
            multiplier = beatCombo.currentRank.rankDamageMultiplier;
            beatCombo.IncreaseComboCounter();
            StartCoroutine(BumpUpMusic());
            StartCoroutine(ChargedAttack());
        }
        else
        {
            GameObject projectile = null;
            GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * damagePenalty);
            projectile.GetComponent<Projectile>().LaunchProjectile(transform.right);
            //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnMissHit-" + randomSound.ToString())).Play();
        }        

    }

    private void SpecialAttackDamage()
    {
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent += SpecialHitOnBeat;
        //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("Special")).Play();
        OnExit += UnsubFromBeat;
    }

    IEnumerator BumpUpMusic()
    {
        BeatManager.Instance.IncreaseMusicVolume(1);

        yield return new WaitForSecondsRealtime(1);

        BeatManager.Instance.DecreaseMusicVolume(0.7f);

        yield break;
    }

    protected void SpecialHitOnBeat()
    {
        float multiplier = 0.8f;
        if (isOnBeat)
        {
            specialMode = true;
            multiplier = beatCombo.currentRank.rankDamageMultiplier;
            beatCombo.IncreaseComboCounter();
        }
        
    }

    protected void UnsubFromBeat()
    {
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent -= SpecialHitOnBeat;
    }

    private void CountBeats()
    {
        if(beats < 7)
        {
            beats++;
        }        
    }

    IEnumerator ChargedAttack()
    {
        StartCoroutine(ChargeAttkAnimation());
        float damageMult = damageMultiplier;
        int randomSound = Random.Range(0, 3);
        GameObject projectile = null;  

        yield return new WaitWhile(() => Input.GetKey(KeyCode.Mouse0));

        if (isOnBeat)
        {
            if (specialMode)
            {
                projectile = GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
                //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnSpecialBeatHit-" + randomSound.ToString())).Play();
                projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * (damageMult * beats));
                projectile.GetComponent<Projectile>().LaunchProjectile(transform.right);

            }
            else
            {
                projectile = GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
                //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnBeatHit-" + randomSound.ToString())).Play();

                switch (beats)
                {
                    case 3:
                        projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * damageMult);                        
                        break;
                    case 5:
                        projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * (damageMult * 2));
                        break;
                    default:
                        projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage);
                        break;
                }

                projectile.GetComponent<Projectile>().LaunchProjectile(transform.right);
            }
        }
        else
        {
            projectile = GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * damagePenalty);
            projectile.GetComponent<Projectile>().LaunchProjectile(transform.right);
        }


            BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent -= CountBeats;
        beats = 0;
        anim.SetInteger("ChargeLevel", 0);
        StopAllCoroutines();


    }

    IEnumerator ChargeAttkAnimation()
    {
        yield return new WaitWhile(() => beats < 1);
        anim.SetInteger("ChargeLevel", 1);
        yield return new WaitWhile(() => beats < 3);
        anim.SetInteger("ChargeLevel", 2);
        yield return new WaitWhile(() => beats < 5);
        anim.SetInteger("ChargeLevel", 3);
    }
}
