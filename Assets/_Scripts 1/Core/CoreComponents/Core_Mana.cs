using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core_Mana : CoreComponent
{
    [SerializeField] private Image manaBar;
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

        if (currentMana == maxMana) isManaFull = true;
    }

    [ContextMenu("FillManaBar")]
    private void TestManaIncrease() => IncreaseMana(100f);
}
