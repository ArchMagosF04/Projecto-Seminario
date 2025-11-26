using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Platform : MonoBehaviour
{
    [field: SerializeField] public bool isPlatformActive {  get; private set; } = true;
    [field: SerializeField] public bool wasPlatformModified { get; private set; } = false;

    [SerializeField] private Vector2 projectileLaunchAngle = Vector2.zero;

    [Header("Components")]
    [SerializeField] private GameObject platformPhysics;
    [SerializeField] private GameObject damagePlatform;
    [SerializeField] private Projectile platDebris;
    
    [SerializeField] private Animator serpentAnim;
    [SerializeField] private Animator mainPlatformAnim;
    private CharacterAnimatorEvent animatorEvent;
    

    private void Awake()
    {
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

        serpentAnim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);
        mainPlatformAnim.SetFloat("BeatSpeedMult", BeatManager.Instance.BeatSpeedMultiplier);

        damagePlatform.SetActive(false);
        TogglePlatform(isPlatformActive);
    }

    private void OnDisable()
    {
        animatorEvent.OnAnimationFinishedTrigger -= DestroyPlatform;
    }

    public void TogglePlatform(bool input)
    {
        isPlatformActive = input;

        platformPhysics.SetActive(isPlatformActive);
        //spriteRenderer.enabled = isPlatformActive;
    }

    [ContextMenu("Set Platform on Fire")]
    public void SetPlatformAflame()
    {
        wasPlatformModified = true;
        mainPlatformAnim.SetBool("Ignite", true);
        StartCoroutine(WaitToSetOnFire());
    }

    private IEnumerator WaitToSetOnFire()
    {
        yield return new WaitForSeconds((60f/BeatManager.Instance.BPM)*2);

        mainPlatformAnim.SetBool("Ignite", false);
        damagePlatform.SetActive(true);
    }

    [ContextMenu("Serpent Attack")]
    public void SerpentTailAttack()
    {
        wasPlatformModified = true;
        animatorEvent.OnAnimationFinishedTrigger += DestroyPlatform;
        serpentAnim.SetTrigger("TailAttack");
    }

    private void SpawnDebris(Vector2 direction)
    {
        Projectile newProjectile = Instantiate(platDebris, transform.position, Quaternion.identity);

        newProjectile.LaunchProjectile(direction);
    }

    private void DestroyPlatform()
    {
        SpawnDebris(projectileLaunchAngle);
        SpawnDebris(new Vector2(-projectileLaunchAngle.x, projectileLaunchAngle.y));

        platformPhysics.SetActive(false);
        mainPlatformAnim.SetBool("Break", true);
        //spriteRenderer.sprite = brokenPlatformSprite;
        //Destroy(gameObject);
    }

    [ContextMenu("Activate Platform")]
    private void PlatTest() => TogglePlatform(true);
}
