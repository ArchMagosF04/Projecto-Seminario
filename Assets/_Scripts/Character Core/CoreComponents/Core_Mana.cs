using Ami.BroAudio;
using Microlight.MicroBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Core_Mana : CoreComponent
{
    [Header("UI")]
    [SerializeField] private Image fullManaBar;
    [SerializeField] private MicroBar microBar;
    [SerializeField] private ParticleSystem manaParticles;

    [Header("Stats")]
    [SerializeField] private float maxMana;
    private float currentMana;

    [Header("Sounds")]
    [SerializeField] private SoundID manaGainSound;
    [SerializeField] private SoundID manaFullSound;

    public bool isManaFull { get; private set; }
    public static Action ManaIsFull = delegate { };

    protected override void Awake()
    {
        base.Awake();
        if (microBar != null) microBar.Initialize(maxMana);
        if (fullManaBar != null) fullManaBar.enabled = false;
    }

    private void Start()
    {
        if (microBar != null) microBar.UpdateBar(0);
        UseMana();
    }

    public void UseMana()
    {
        currentMana = 0f;
        isManaFull = false;
        if (fullManaBar != null) fullManaBar.enabled = false;
        if (microBar != null) microBar.UpdateBar(currentMana, UpdateAnim.Damage);
    }

    public void IncreaseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        if (microBar != null) microBar.UpdateBar(currentMana, UpdateAnim.Heal);

        if (currentMana == maxMana && !isManaFull)
        {
            if (manaFullSound.IsValid()) BroAudio.Play(manaFullSound);
            isManaFull = true;
            if (fullManaBar != null) fullManaBar.enabled = true;
            if (microBar != null) microBar.UpdateBar(currentMana, UpdateAnim.MaxHP);
            //FullAnimation();
            ManaIsFull();
        }

        if (!isManaFull)
        {
            manaParticles.Play();
            if (manaGainSound.IsValid()) BroAudio.Play(manaGainSound);
        }
    }

    [ContextMenu("FillManaBar")]
    private void TestManaIncrease() => IncreaseMana(100f);
}
