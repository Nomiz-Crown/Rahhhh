using UnityEngine;
using TMPro;
using System.Collections;

public class Engineer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI talkPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public UnityEngine.UI.Button continueButton;
    public RectTransform portrait; // assign the portrait Image's RectTransform here (om det inte är rectform INSERTA INTE DEN DIN HUMONCULUS

    [Header("Dialogue")]
    [TextArea(3, 5)]
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    public float portraitBobAmount = 5f; // pixels
    public float portraitBobSpeed = 20f;  // pixels per second

    [Header("State")]
    public bool isTalking = false;

    private bool playerInRange = false;
    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;

    private Vector3 portraitOriginalPos;

    void Start()
    {
        talkPrompt.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);

        continueButton.onClick.AddListener(NextLine);

        if (portrait != null)
            portraitOriginalPos = portrait.localPosition;
    }

    void Update()
    {
        // logic jag Gillar, du kan ändra om du intish gillish
        talkPrompt.gameObject.SetActive(playerInRange && !isTalking);

        // Start talking med engineer snubbe gubben
        if (playerInRange && !isTalking && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;

        StartTyping(dialogueLines[currentLineIndex]);
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line));
    }

    IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        continueButton.gameObject.SetActive(false);

        float timer = 0f; // used for portrait bobbing
        foreach (char letter in line)
        {
            dialogueText.text += letter;

            // bobbing bobbing bobbign
            if (portrait != null)
            {
                timer += Time.deltaTime * portraitBobSpeed;
                portrait.localPosition = portraitOriginalPos + Vector3.up * Mathf.Sin(timer) * portraitBobAmount;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Reset portrait
        if (portrait != null)
            portrait.localPosition = portraitOriginalPos;

        continueButton.gameObject.SetActive(true);
    }

    public void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
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
        isTalking = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (portrait != null)
            portrait.localPosition = portraitOriginalPos;
        // Create quest after dialogue ends, MURRELBRADT, KEVIN LUSTYCH
        Quest newQuest = new Quest("Engineer", "Find a Wrench", "Turret BluePrint");
        Debug.Log(newQuest.objective + " reward: " + newQuest.reward + " quest from: " + newQuest.questGiver);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
