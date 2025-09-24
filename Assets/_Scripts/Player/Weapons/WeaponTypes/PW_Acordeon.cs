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

    [Tooltip("Duracion del modo especial en segundos")]
    [SerializeField] private float specialModeDuration = 10;
    private float currentSpecialDuration;

    [Tooltip("Aumento de daño base a cada proyectil durante el modo especial. Formula: (basicAttackDamage + specialModeDamageBomus) * damageMultiplier")]
    [SerializeField] private float specialModeDamageBonus;

    [Tooltip("PORCENTAJE de Aumento de velocidad en los projectiles durante el modo especial")]
    [SerializeField] private float projectileSpeedBonus = 20;

    [Tooltip("Multiplicador de daño por cada nivel de carga cuando se mantiene apretado el boton de ataque. Formula: DañoBasico * (Multiplicador * Nivel de carga)")]
    [SerializeField] private float damageMultiplier = 1.5f;

    [Tooltip("Multiplicador de daño cuando se falla el ataque cargado. Formula: DañoBasico * penaltyMultiplier. \n Tabla de Referencia:\n 0.25= 75% menos daño\n 0.5 = 50% menos daño\n 0.75 = 25% menos daño\n 1 = AtaqueNormal sin reduccion")]
    [SerializeField] private float damagePenalty = 0.5f;

    [Tooltip("Cantidad de beats de gracia antes que el ataque cargado se falle automaticamente luego de alcanzar el maximo de cargas")]
    [SerializeField] private int graceBeats = 2;    

    [Tooltip("Cual es la frecuencia de beat que este arma reconoce como correcto")]
    [SerializeField] private BeatEnumarator designatedBeat = 0;

    [Tooltip("Cuanto mana recargara cada impacto de proyectil")]
    [SerializeField] protected float manaOnBeatHit;

    //[SerializeField] protected SoundLibraryObject soundLibrary;

    [Tooltip("Lista de prefabs de proyectiles del arma. El arma utiliza un random para elegir uno de estos proyectiles cada vez que dispara")]
    [SerializeField] private GameObject[] projectiles;

    

    private int beats;
    private bool specialMode;
    [SerializeField]private int charge = 0;
    private bool charging = false;

    protected override void Awake()
    {
        base.Awake();
        //BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent
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
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent -= CountBeats;
    }

    private void Update()
    {       
        if (!specialMode && beats>=5 + graceBeats)
        {
            int randomSound = Random.Range(0, 3);
            Shoot(randomSound, basicAttackDamage, false, false, damagePenalty);
            beatCombo.OnTimerDecay();
        }
        else if (specialMode && beats >= 3 + graceBeats)
        {            
            int randomSound = Random.Range(0, 3);
            Shoot(randomSound, basicAttackDamage, false, false, damagePenalty);
            beatCombo.OnTimerDecay();
        }

        if (currentSpecialDuration > 0)
        {
            currentSpecialDuration -= Time.deltaTime;            
        }
        else if (currentSpecialDuration <= 0 && specialMode == true)
        {
            specialMode = false;
            currentSpecialDuration = 0;
            anim.SetBool("Special2", false);
        }
    }

    private void BasicAttackDamage()
    {
        int randomSound = Random.Range(0, 3);

        if (isOnBeat)
        {            
            float multiplier = 0.8f;
            multiplier = beatCombo.currentRank.rankDamageMultiplier;
            beatCombo.IncreaseComboCounter();
            StartCoroutine(BumpUpMusic());
            StartCoroutine(ChargedAttack());
        }
        else
        {
            Shoot(randomSound, basicAttackDamage, false, false, damagePenalty);
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

    private void CountBeats()
    {
        if(!specialMode && beats < 5 + graceBeats)
        {
            beats++;

            switch (beats)
            {
                case 2:
                    charge++;
                    break;
                case 4:
                    charge++;
                    break;
                default:                    
                    break;
            }
        }
        else if(specialMode && beats < 3 + graceBeats)
        {
            beats++;

            switch (beats)
            {
                case 0:
                    charge++;
                    break;
                case 2:
                    charge++;
                    break;
                default:
                    break;
            }
        }

        if (charging && beats < 5)
        {
            float multiplier = 0.8f;
            multiplier = beatCombo.currentRank.rankDamageMultiplier;
            beatCombo.IncreaseComboCounter();
        }
    }

    IEnumerator ChargedAttack()
    {
        charging = true;
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent += CountBeats;
        StartCoroutine(ChargeAttkAnimation());
        float damageMult = damageMultiplier;
        int randomSound = Random.Range(0, 3);

        yield return new WaitWhile(() => Input.GetKey(KeyCode.Mouse0));

        if (BeatManager.Instance.BeatGracePeriod) isOnBeat = true;

        if (isOnBeat)
        {
            Debug.Log("Atack on beat");
            if (specialMode)
            {               
                switch (beats)
                {
                    case 1:                        
                        Shoot(randomSound, basicAttackDamage + specialModeDamageBonus, true, true, damageMult);

                        break;
                    case 3:                        
                        Shoot(randomSound, basicAttackDamage + specialModeDamageBonus, true, true, damageMult*2);
                        break;
                    default:
                        if (beats > 3) Shoot(randomSound, basicAttackDamage + specialModeDamageBonus, true, true, damageMult * 2);

                        else                            
                            Shoot(randomSound, basicAttackDamage + specialModeDamageBonus, true, true);
                        break;
                }
            }
            else
            {
                switch (beats)
                {
                    case 3:                        
                        Shoot(randomSound, basicAttackDamage, true, false, damageMult);                        
                        break;
                    case 5:                       
                        Shoot(randomSound, basicAttackDamage, true, false, damageMult*2);
                        break;
                    default:

                        if(beats > 5) Shoot(randomSound, basicAttackDamage, true, false, damageMult * 2);
                        else
                            Shoot(randomSound, basicAttackDamage, true, false);
                        break;
                }                
            }
        }
        else
        {
            Shoot(randomSound, basicAttackDamage, true, false, damagePenalty);
        }
    }

    IEnumerator ChargeAttkAnimation()
    {
        //yield return new WaitWhile(() => charge < 1);
        anim.SetInteger("ChargeLevel", 1);
        yield return new WaitWhile(() => charge == 0);
        anim.SetInteger("ChargeLevel", 2);
        yield return new WaitWhile(() => charge > 1);
        anim.SetInteger("ChargeLevel", 3);
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

        projectile = GameObject.Instantiate(projectiles[projectileNumber], transform.position, transform.rotation);

        projectile.GetComponent<Projectile>().SetDamage(damage * mod);


        if (special)
        {
            float s = 0;
            s = projectile.GetComponent<Projectile>().GetSpeed();
            float extraSpeed = (projectileSpeedBonus * s) / 100;
            projectile.GetComponent<Projectile>().SetSpeed(s + extraSpeed);
        }
        else
        {
            if (onBeat) projectile.GetComponent<Projectile>().OnHit += RecoverMana;
        }

        if (onBeat)
        {

        }
        else
        {
            projectile.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }

        //SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnSpecialBeatHit-" + projectileNumber.ToString())).Play();
        projectile.GetComponent<Projectile>().LaunchProjectile(transform.right);

        charging = false;
        BeatManager.Instance.intervals[((int)designatedBeat)].OnBeatEvent -= CountBeats;
        beats = 0;
        anim.SetInteger("ChargeLevel", 0);
        charge = 0;
        StopAllCoroutines();
    }
}
