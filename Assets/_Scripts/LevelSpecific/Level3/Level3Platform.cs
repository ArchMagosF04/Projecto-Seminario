using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Platform : MonoBehaviour
{
    [field: SerializeField] public bool isPlatformActive {  get; private set; } = false;
    [field: SerializeField] public bool wasPlatformModified { get; private set; } = false;

    [Header("Components")]
    [SerializeField] private GameObject platformPhysics;
    [SerializeField] private GameObject damagePlatform;
    
    private Animator serpentAnim;
    private CharacterAnimatorEvent animatorEvent;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        serpentAnim = GetComponentInChildren<Animator>();
        animatorEvent = GetComponentInChildren<CharacterAnimatorEvent>();

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
        spriteRenderer.enabled = isPlatformActive;
    }

    [ContextMenu("Set Platform on Fire")]
    public void SetPlatformAflame()
    {
        wasPlatformModified = true;
        spriteRenderer.color = Color.red;
        damagePlatform.SetActive(true);
    }

    [ContextMenu("Serpent Attack")]
    public void SerpentTailAttack()
    {
        wasPlatformModified = true;
        animatorEvent.OnAnimationFinishedTrigger += DestroyPlatform;
        serpentAnim.SetTrigger("TailAttack");
    }

    private void DestroyPlatform()
    {
        Destroy(gameObject);
    }

    [ContextMenu("Activate Platform")]
    private void PlatTest() => TogglePlatform(true);
}
