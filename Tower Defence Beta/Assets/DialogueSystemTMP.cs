using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystemTMP : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text dialogueText;
    public Button continueButton;

    [Header("Typing")]
    public float letterSpeed = 0.04f;

    [Header("Dialogue")]
    [TextArea(3, 5)]
    public List<string> dialogueLines;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);
        gameObject.SetActive(false);

        continueButton.onClick.AddListener(NextDialogue);
    }

    /// <summary>
    /// Call this from trigger / NPC / button / E key
    /// </summary>
    public void StartDialogue()
    {
        if (dialogueActive) return;

        dialogueActive = true;
        currentLineIndex = 0;

        gameObject.SetActive(true);
        StartTyping(dialogueLines[currentLineIndex]);
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterSpeed);
        }

        isTyping = false;
        continueButton.gameObject.SetActive(true);
    }

    public void NextDialogue()
    {
        if (!dialogueActive) return;

        // Skip typing
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
            continueButton.gameObject.SetActive(true);
            return;
        }

        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Count)
        {
            StartTyping(dialogueLines[currentLineIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // OPTIONAL: Allow E key to advance dialogue
    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            NextDialogue();
        }
    }
}
