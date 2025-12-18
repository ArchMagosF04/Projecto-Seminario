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
    [SerializeField] private Image manaBar;
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
        if (manaBar != null)
        {
            manaBar.fillAmount = 0f;
            manaBar.GetComponent<Animator>().SetLayerWeight(1, 0);
        }
        if (microBar != null) microBar.UpdateBar(currentMana, UpdateAnim.Damage);
    }

    public void IncreaseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        if (manaBar != null) manaBar.fillAmount = currentMana / maxMana;
        if (microBar != null) microBar.UpdateBar(currentMana, UpdateAnim.Heal);

        if (currentMana == maxMana && !isManaFull)
        {
            if (manaFullSound.IsValid()) BroAudio.Play(manaFullSound);
            isManaFull = true;
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

    private void FullAnimation()
    {
        manaBar.GetComponent<Animator>().SetLayerWeight(1,1);
    }

    private void AdvanceAnimation(bool result)
    {
        manaBar.GetComponent<Animator>().SetTrigger("onBeat");
    }


}
