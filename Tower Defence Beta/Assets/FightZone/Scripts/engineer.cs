using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class engineer : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public Text dialogueText;           
    public string[] dialogueLines;     
    public float typingSpeed = 0.05f;  

    private int currentLine = 0;
    private bool isTyping = false;

    [Header("UI & Interaction")]
    public GameObject newObjectToActivate;

    [HideInInspector]
    public bool isInConversation = false;

    public GameObject uiTextObject; //JAG ÄLSKAR PEPSI


    public GameObject targetObject;

    public float scaleAmount = 1.2f;
    public float scaleSpeed = 3f;

    private static bool hasPlayed = false;

    void Start()
    {
        if (uiTextObject != null)
            uiTextObject.SetActive(false);

        if (hasPlayed)
            return;

        hasPlayed = true;
        StartCoroutine(ActivateAndAnimate());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (uiTextObject != null && uiTextObject.activeSelf)
            {
                if (newObjectToActivate != null)
                {
                    newObjectToActivate.SetActive(true);

                    if (dialogueLines.Length > 0 && dialogueText != null)
                    {
                        currentLine = 0;
                        StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
                    }
                }

                // Start conversation
                isInConversation = true;

                // Hide the UI text, uhh like the där E tro talk grejen
                uiTextObject.SetActive(false);

                // Activate the new object LALAL
                if (newObjectToActivate != null)
                    newObjectToActivate.SetActive(true);
            }
        }
    }

    IEnumerator ActivateAndAnimate()
    {
        targetObject.SetActive(true);

        Vector3 originalScale = targetObject.transform.localScale;
        Vector3 biggerScale = originalScale * scaleAmount;

        yield return StartCoroutine(ScaleTo(biggerScale));
        yield return StartCoroutine(ScaleTo(originalScale));
        yield return StartCoroutine(ScaleTo(biggerScale));
        yield return StartCoroutine(ScaleTo(originalScale));

        yield return new WaitForSeconds(5f);

        targetObject.SetActive(false);
    }

    IEnumerator ScaleTo(Vector3 targetScale)
    {
        while (Vector3.Distance(targetObject.transform.localScale, targetScale) > 0.01f)
        {
            targetObject.transform.localScale = Vector3.Lerp(
                targetObject.transform.localScale,
                targetScale,
                Time.deltaTime * scaleSpeed
            );
            yield return null;
        }

        targetObject.transform.localScale = targetScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isInConversation)
        {
            if (uiTextObject != null)
                uiTextObject.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the UI text. i love pepsi
            if (uiTextObject != null)
                uiTextObject.SetActive(false);

            // If in conversation, end it when player leaves
            if (isInConversation)
            {
                isInConversation = false;

                if (newObjectToActivate != null)
                    newObjectToActivate.SetActive(false);
            }
        }
    }
    private IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    public void ShowNextDialogue()
    {
        if (isTyping)
        {
            // Finish current line instantly if still typing, älskar detta
            StopAllCoroutines();
            dialogueText.text = dialogueLines[currentLine];
            isTyping = false;
            return;
        }

        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(TypeDialogue(dialogueLines[currentLine]));
        }
        else
        {
            // End of dialogue, hide box and reset
            newObjectToActivate.SetActive(false);
            isInConversation = false;
            currentLine = 0;
        }
    }



}
