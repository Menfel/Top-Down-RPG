using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private Coroutine typingCoroutine;

    public void Interact()
    {
        if (dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
            return;

        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else
        {
            if (isTyping)
            {
                // Прерываем корутину и показываем весь текст
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);

                // Воспроизводим один звук, если надо
                if (dialogueData.voiceSound != null)
                    SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);

                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        typingCoroutine = StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (dialogueIndex < dialogueData.dialogueLines.Length - 1)
        {
            dialogueIndex++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;

            if (dialogueData.voiceSound != null)
                SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);

            yield return new WaitForSecondsRealtime(Mathf.Max(0.01f, dialogueData.typingSpeed));

        }

        isTyping = false;

        // Автоматическое продолжение, если включено
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }
}
