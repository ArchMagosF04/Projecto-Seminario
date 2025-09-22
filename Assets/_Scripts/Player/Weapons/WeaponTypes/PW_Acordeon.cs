using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PW_Acordeon : PlayerWeapon
{
    [Header("Weapon Stats")]

    
    [SerializeField] protected float basicAttackDamage = 3f; [Tooltip("Daño de cada proyectil")]
    [SerializeField] private float damageMultiplier = 1.5f; [Tooltip("Multiplicador de daño por cada beat que se mantiene apretado el boton de ataque. Formula: basicAttackDamage * (damageMult * beats)")]
    //[SerializeField] protected float specialAttackSpeed = 1f;
    [SerializeField] protected float manaOnBeatHit;
    [SerializeField] protected SoundLibraryObject soundLibrary;
    [SerializeField] private GameObject[] projectiles;

    private int beats;
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
        BeatManager.Instance.intervals[2].OnBeatEvent -= SpecialHitOnBeat;
        BeatManager.Instance.intervals[2].OnBeatEvent -= CountBeats;
        OnExit -= UnsubFromBeat;

    }

    private void BasicAttackDamage()
    {
        int randomSound = Random.Range(0, 3);

        if (isOnBeat)
        {
            BeatManager.Instance.intervals[2].OnBeatEvent += CountBeats;
            float multiplier = 0.8f;
            multiplier = beatCombo.currentRank.rankDamageMultiplier;
            beatCombo.IncreaseComboCounter();
            StartCoroutine(BumpUpMusic());
            StartCoroutine(ChargedAttack());
        }
        else
        {
            GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnMissHit-" + randomSound.ToString())).Play();
        }        

    }

    private void SpecialAttackDamage()
    {
        BeatManager.Instance.intervals[2].OnBeatEvent += SpecialHitOnBeat;
        SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("Special")).Play();
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
        BeatManager.Instance.intervals[2].OnBeatEvent -= SpecialHitOnBeat;
    }

    private void CountBeats()
    {
        if(beats < 5)
        {
            beats++;
        }        
    }

    IEnumerator ChargedAttack()
    {
        float damageMult = damageMultiplier;
        int randomSound = Random.Range(0, 3);
        GameObject projectile = null;  

        yield return new WaitWhile(() => Input.GetKey(KeyCode.Mouse0));       

        if (specialMode)
        {
            projectile = GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnSpecialBeatHit-" + randomSound.ToString())).Play();
            projectile.GetComponent<Projectile>().SetDamage(basicAttackDamage * (damageMult * beats));

        }
        else
        {
            projectile = GameObject.Instantiate(projectiles[randomSound], transform.position, transform.rotation);
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("OnBeatHit-" + randomSound.ToString())).Play();

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
        }

        BeatManager.Instance.intervals[2].OnBeatEvent -= CountBeats;
        beats = 0;


    }
}
