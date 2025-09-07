using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Core_Mana : CoreComponent
{
    [Header("UI")]
    [SerializeField] private Image manaBar;

    [Header("Stats")]
    [SerializeField] private float maxMana;
    private float currentMana;

    public bool isManaFull { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        UseMana();
    }

    public void UseMana()
    {
        currentMana = 0f;
        isManaFull = false;
        manaBar.fillAmount = 0f;
    }

    public void IncreaseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        manaBar.fillAmount = currentMana / maxMana;

        if (currentMana == maxMana && !isManaFull)
        {
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("EnergyFull")).Play();
            isManaFull = true;
        }

        if (!isManaFull)
        {
            SoundManager.Instance.CreateSound().WithSoundData(soundLibrary.GetSound("ManaUp01")).Play();
        }
    }

    [ContextMenu("FillManaBar")]
    private void TestManaIncrease() => IncreaseMana(100f);
}
