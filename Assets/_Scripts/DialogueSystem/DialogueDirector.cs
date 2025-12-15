using Ami.BroAudio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueDirector : MonoBehaviour
{
    #region Component References

    [Header("UI Components")]
    [SerializeField] private GameObject leftDialogueBox;
    [SerializeField] private GameObject rightDialogueBox;

    [Space(5)]

    [SerializeField] private Image leftDialogueImage;
    [SerializeField] private Image rightDialogueImage;

    [Space(5)]

    [SerializeField] private TMP_Text leftDialogueSpeakerNameText;
    [SerializeField] private TMP_Text rightDialogueSpeakerNameText;

    [Space(5)]

    [SerializeField] private TMP_Text leftDialogueText;
    [SerializeField] private TMP_Text rightDialogueText;

    [Space(5)]

    [SerializeField] private SO_DialogueScene dialogueScene;

    #endregion

    private PlayerInput playerInput;

    private int currentDialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

    private GameObject currentTextBox;
    private TMP_Text currentDialogueText;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();

        leftDialogueBox.SetActive(false);
        rightDialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            if (DialogueInputManager.Instance.NextDialogueInput)
            {
                DialogueInputManager.Instance.UseNextDialogueInput();
                NextLine();
            }

            if (DialogueInputManager.Instance.SkipDialogueInput)
            {
                DialogueInputManager.Instance.UseSkipDialogueInput();
                EndDialogue();
            }
        }
    }

    public void SetDialogueScene(SO_DialogueScene dialogueScene)
    {
        if (isDialogueActive) return;
        this.dialogueScene = dialogueScene;
    }

    public void StartDialogue()
    {
        if (dialogueScene == null || isDialogueActive) return;

        isDialogueActive = true;
        GameManager.Instance.ToggleGameActiveState(false);
        playerInput.SwitchCurrentActionMap("Dialogue");

        currentDialogueIndex = 0;

        ProgressToNextDialogue();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();

        leftDialogueBox.SetActive(false);
        rightDialogueBox.SetActive(false);

        isDialogueActive = false;
        playerInput.SwitchCurrentActionMap("Player");
        GameManager.Instance.ToggleGameActiveState(true);
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            currentDialogueText.SetText(dialogueScene.dialogueLines[currentDialogueIndex].dialogueLines);
            isTyping = false;
        }
        else if (currentDialogueIndex + 1 < dialogueScene.dialogueLines.Length)
        {
            if (dialogueScene.dialogueLines[currentDialogueIndex].speakerName != dialogueScene.dialogueLines[currentDialogueIndex+1].speakerName)
                currentTextBox.SetActive(false);

            currentDialogueIndex++;
            ProgressToNextDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ProgressToNextDialogue()
    {
        DialogueBoxSide nextSide = dialogueScene.dialogueLines[currentDialogueIndex].displaySide;

        if (nextSide == DialogueBoxSide.Left)
            StartCoroutine(TypeLine(leftDialogueBox, leftDialogueImage, leftDialogueSpeakerNameText, leftDialogueText));
        else if (nextSide == DialogueBoxSide.Right)
            StartCoroutine(TypeLine(rightDialogueBox, rightDialogueImage, rightDialogueSpeakerNameText, rightDialogueText));
    }

    private IEnumerator TypeLine(GameObject dialogueBox, Image dialogueImage, TMP_Text speakerNameText, TMP_Text dialogueText)
    {
        currentTextBox = dialogueBox;
        currentDialogueText = dialogueText;

        DialogueLine currentLine = dialogueScene.dialogueLines[currentDialogueIndex];
        dialogueText.SetText("");

        dialogueImage.sprite = currentLine.speakerTextBoxSprite;
        speakerNameText.text = currentLine.speakerName;
        dialogueBox.SetActive(true);

        isTyping = true;

        foreach (char letter in currentLine.dialogueLines)
        {
            dialogueText.text += letter;
            if (currentLine.voiceSound.IsValid()) BroAudio.Play(currentLine.voiceSound);
            yield return new WaitForSeconds(currentLine.typingSpeed);
        }

        isTyping = false;

        if (currentLine.autoProgressDialogue)
        {
            yield return new WaitForSeconds(currentLine.autoProgressDelay);

            NextLine();
        }
    }
}
