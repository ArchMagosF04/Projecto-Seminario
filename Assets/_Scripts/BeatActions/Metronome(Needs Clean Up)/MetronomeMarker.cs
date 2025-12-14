using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeMarker : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool isMoving;

    [Header("MovementParameters")]
    [SerializeField] private float speed = 0;
    [SerializeField] private float distance = 1.25f;
    [SerializeField] private float timePerBeat;
    

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        timePerBeat = 60 / BeatManager.Instance.BPM;
        speed = distance / timePerBeat;
    }

    private void OnEnable()
    {
        isMoving = false;
        spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
    }

    public void ChangeMarkerColor(Color newColor) => spriteRenderer.color = newColor;

    public void ToggleMovility(bool toggle) => isMoving = toggle;
}
