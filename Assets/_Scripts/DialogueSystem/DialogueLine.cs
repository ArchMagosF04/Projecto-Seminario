using Ami.BroAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [Header("Main Text Box")]
    public string speakerName;
    public Sprite speakerTextBoxSprite;
    [Tooltip("Character Limit of 160")]
    [TextArea] public string dialogueLines;
    public DialogueBoxSide displaySide;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;
    public SoundID voiceSound;
    public bool autoProgressDialogue = true;
    public float autoProgressDelay = 1.5f;
}

public enum DialogueBoxSide
{
    Left, Right
}
