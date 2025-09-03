using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core_Mana : CoreComponent
{
    [SerializeField] private Image manaBar;
    [SerializeField] private float maxMana;
    private float currentMana;

    private GameObject player;

    public bool isManaFull { get; private set; }
    private bool fullSoundPlayed = false;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        UseMana();
    }

    public void UseMana()
    {
        currentMana = 0f;
        isManaFull = false;
        fullSoundPlayed=false;
        manaBar.fillAmount = 0f;
    }

    public void IncreaseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        manaBar.fillAmount = currentMana / maxMana;

        if (currentMana == maxMana) isManaFull = true;

        if(player != null)
        {
            if (!isManaFull)
            {
                player.GetComponentInParent<PlayerController>().PlaySound("ManaUp01");
            }
            else if (isManaFull /*&& fullSoundPlayed == false*/)
            {
                player.GetComponentInParent<PlayerController>().PlaySound("EnergyFull");
                //fullSoundPlayed = true;
            }
        }
    }

    [ContextMenu("FillManaBar")]
    private void TestManaIncrease() => IncreaseMana(100f);
}
