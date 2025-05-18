using UnityEngine;

public class Death : CoreComponent
{
    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;

    private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
    private Stats stats;

    [SerializeField] private GameObject[] deathParticles;

    public void Die()
    {
        //foreach (var particle in deathParticles)
        //{
        //    ParticleManager.StartParticles(particle);
        //}

        Debug.Log("Dead");

        core.transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Stats.Health.OnCurrentValueZero += Die;
    }

    private void OnDisable()
    {
        Stats.Health.OnCurrentValueZero -= Die;
    }
}
