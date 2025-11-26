using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollUp : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float startScrollTime = 1f;

    public bool ShouldMove;

    private float startMoveBuffer;

    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        startMoveBuffer = startScrollTime;
    }

    private void Update()
    {
        if (!dialogueManager.introEnded) return;

        if (ShouldMove)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else
        {
            startMoveBuffer -= Time.deltaTime;

            if (startMoveBuffer <= 0)
            {
                ShouldMove = true;
                startMoveBuffer = startScrollTime;
            }
        }
    }

    public void ToggleMovent(bool value) { ShouldMove = value; }
}
