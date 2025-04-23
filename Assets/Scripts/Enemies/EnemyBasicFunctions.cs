using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicFunctions : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    private float hp;
    private float spd;
    private float atk;
    [SerializeField] private bool useOwnGracePeriod;
    [SerializeField] private float gracePeriodForBeat;
    private float graceTimer;

    public float beatDamageMultiplier; //Despues Mover esto al GameManager

    public static bool onBeat;
   
    void Start()
    {
        hp = stats.Hp;
        spd = stats.Spd;
        atk = stats.Atk;
        gameObject.GetComponent<BeatDetector>().OnBeat += ActivateBeatEffect;
    }

    void Update()
    {
        if (onBeat)
        {
            if (graceTimer < gracePeriodForBeat)
            {
                graceTimer += Time.deltaTime;
            }
            else if (graceTimer >= gracePeriodForBeat)
            {
                onBeat = false;
                graceTimer = 0;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (hp > 0)
        {
            if (!onBeat)
            {
                hp -= damage;
            }
            else
            {
                hp -= damage * beatDamageMultiplier;
            }

        }
    }

    private void ActivateBeatEffect(bool activate)
    {
        onBeat =activate;
    }
        
}
