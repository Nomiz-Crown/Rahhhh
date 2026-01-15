using UnityEngine;
using TMPro;
using System.Collections;

public class Engineer : MonoBehaviour
{
    public GameObject Turret;
    public bool isQuestComplete = false;

    public static Engineer instance;
    [Header("UI")]
    public TextMeshProUGUI talkPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public UnityEngine.UI.Button continueButton;
    public RectTransform portrait; // assign the portrait Image's RectTransform here (om det inte är rectform INSERTA INTE DEN DIN HUMONCULUS

    public float typingSpeed = 0.05f;
    public float portraitBobAmount = 5f; // pixels
    public float portraitBobSpeed = 20f;  // pixels per second

    [Header("Dialogue")]
    [TextArea(3, 5)]
    public string[] dialogueLines;

    [Header("Dialogue Sets")]
    [TextArea(3, 5)]
    public string[] dialogue_NoWrench;

    [TextArea(3, 5)]
    public string[] dialogue_WithWrench;
    
    [Header("State")]
    public bool isTalking = false;

    private bool playerInRange = false;
    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;

    private Vector3 portraitOriginalPos;

    public bool playerHasWrench = false;
    void Start()
    {
        Turret.SetActive(false);
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

    void Awake()
    {
        instance = this;
    }
        void StartDialogue()
        {
            isTalking = true;
            dialoguePanel.SetActive(true);
            currentLineIndex = 0;

            if (playerHasWrench)
                dialogueLines = dialogue_WithWrench;
            else
                dialogueLines = dialogue_NoWrench;

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

            if (playerHasWrench)
            {
                Debug.Log("rub m belly, thanksss");
                isQuestComplete = true; 
                Turret.SetActive(true);
            }
            else
            {
                Debug.Log("Where is my wreinch ??????");
            }
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
