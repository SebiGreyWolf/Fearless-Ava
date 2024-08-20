using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text nameText; // The UI element for displaying the NPC's name
    public Text dialogueText; // The UI element for displaying the dialogue text
    public GameObject dialogueBox; // The UI container for the dialogue system
    public GameObject questUI; // The UI container for the quest list
    public TextMeshProUGUI questListText; // The UI element for displaying the active quests

    private Queue<string> sentences; // A queue to store sentences of the current dialogue
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidbody;
    private Quest currentQuestToUse;

    private NoQuestDialogue tempNoQuestTrigger;
    private DialogueTrigger currentTrigger;
    void Start()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(false); // Ensure the dialogue box is hidden initially

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerRigidbody = playerMovement.GetComponent<Rigidbody2D>();

        currentQuestToUse = null;
        tempNoQuestTrigger = null;
    }
    void Update()
    {
        // Check for spacebar input to display the next sentence
        if (Input.GetKeyDown(KeyCode.Space) && dialogueBox.activeSelf)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(DialogueTrigger trigger, Quest currentQuest)
    {
        playerMovement.enabled = false;
        playerRigidbody.velocity = Vector2.zero;

        currentTrigger = trigger;

        currentQuestToUse = currentQuest;

        // Clear previous dialogue sentences
        sentences.Clear();
        dialogueBox.SetActive(true);

        // Enqueue the new sentences for the current quest state
        string[] dialogueSentences = trigger.GetDialogueForQuest(currentQuest);
        foreach (string sentence in dialogueSentences)
        {
            sentences.Enqueue(sentence);
        }

        // Display the first sentence
        DisplayNextSentence();
    }

    public void StartNoQuestDialogue(DialogueTrigger trigger, string[] sentencesToUse)
    {
        playerMovement.enabled = false;
        playerRigidbody.velocity = Vector2.zero;

        currentTrigger = trigger;

        // Clear previous dialogue sentences
        sentences.Clear();
        dialogueBox.SetActive(true);

        // Set the NPC's name in the UI
        nameText.text = trigger.GetCurrentSpeakerName();

        // Enqueue the non-quest dialogue sentences
        foreach (string sentence in sentencesToUse)
        {
            sentences.Enqueue(sentence);
        }

        // Display the first sentence
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // Check if there are more sentences to display
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        nameText.text = currentTrigger.GetCurrentSpeakerName();
        // Get the next sentence from the queue
        string sentence = sentences.Dequeue();

        // Stop any ongoing typing effect and start typing the new sentence
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = ""; // Clear the dialogue text field

        // Type out the sentence character by character
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // Wait until the next frame
        }
    }

    public void EndDialogue()
    {
        // Hide the dialogue box
        dialogueBox.SetActive(false);

        // Re-enable player movement
        playerMovement.enabled = true;

        // Check if we are dealing with a quest-related dialogue
        if (currentQuestToUse != null)
        {
            // Handle quest activation or completion logic
            QuestManager questManager = FindObjectOfType<QuestManager>();

            if (!currentQuestToUse.isActive)
            {
                // Activate the quest if it was not active before
                questManager.ActivateQuest(currentQuestToUse);
            }

            if (currentQuestToUse.isCompleted)
            {
                // If the quest is completed, remove it
                questManager.RemoveQuest(currentQuestToUse);
                currentQuestToUse = null;
            }

            // Update the quest UI if there was a change to the quest state
            UpdateQuestUI();
        }
        else if (currentTrigger != null)
        {
            // If it's not a quest dialogue, check for reward associated with the no-quest dialogue
            currentTrigger.HandleReward();
            currentTrigger.RemoveDialogue();
        }

        // Optionally reset the current trigger reference
        currentTrigger = null;
    }

    public void UpdateQuestUI()
    {
        // Retrieve the current active quests from the QuestManager
        QuestManager questManager = FindObjectOfType<QuestManager>();
        List<Quest> activeQuests = questManager.allQuests.FindAll(q => q.isActive);
        // Clear the quest list UI text
        questListText.text = "";

        // Display each active quest and its completion status
        foreach (Quest quest in activeQuests)
        {
            if (quest.showInUI) // Only include quests that should be shown in the UI
            {
                string questStatus = quest.isCompleted ? "<s>" + quest.questName + "</s>" : quest.questName;
                questListText.text += questStatus + "\n";

                foreach (Item item in quest.requiredItems)
                {
                    string itemStatus = item.currentCount >= item.maxCount ? "<s>" + item.itemName + ": " + item.currentCount + "/" + item.maxCount + "</s>" : item.itemName + ": " + item.currentCount + "/" + item.maxCount;
                    questListText.text += "  - " + itemStatus + "\n";
                }
            }
        }

        // Show the quest UI if there are active quests
        //questUI.SetActive(activeQuests.Count > 0);
    }
    public void ClearDialogue()
    {
        questListText.text = "";
    }
}