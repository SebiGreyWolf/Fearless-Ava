using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;
    public GameObject questUI;
    public TextMeshProUGUI questListText; // Reference to the UI Text component that displays the quest list
    private Queue<string> sentences;

    private DialogueTrigger currentDialogueTrigger;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidbody;
    void Start()
    {
        sentences = new Queue<string>();
        ClearQuestUI();

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerRigidbody = playerMovement.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // Check if the space bar is pressed to continue dialogue
        if (Input.GetKeyDown(KeyCode.Space) && dialogueBox.activeSelf)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger)
    {
        dialogueBox.SetActive(true); // Activate the dialogue box
        currentDialogueTrigger = dialogueTrigger;

        sentences.Clear();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            playerRigidbody.velocity = Vector2.zero;
        }

        foreach (string sentence in dialogueTrigger.GetCurrentSentences())
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        nameText.text = currentDialogueTrigger.GetCurrentSpeakerName();
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (currentDialogueTrigger != null)
        {
            currentDialogueTrigger.ActivateQuest();
            if (currentDialogueTrigger.RemoveQuest())
            {
                ClearQuestUI();
            }
        }
    }

    public void UpdateQuestUI(Quest quest)
    {
        ClearQuestUI();

        bool allItemsCompleted = true; // Flag to check if all items are completed
        string questText = "";

        // Check if the quest is completed
        if (quest.isCompleted)
        {
            questText += "<s>"; // Start strikethrough tag for the entire quest
        }

        questText += $"<b>{quest.questName}</b>:\n"; // Quest name in bold

        // Iterate through each required item
        foreach (Item item in quest.requiredItems)
        {
            if (item.currentCount >= item.maxCount)
            {
                questText += "<s>"; // Start strikethrough tag for the item
            }
            else
            {
                allItemsCompleted = false; // Set flag to false if any item is incomplete
            }

            questText += $"{item.name}: {item.currentCount}/{item.maxCount}\n"; // Item name and amount

            if (item.currentCount >= item.maxCount)
            {
                questText += "</s>"; // End strikethrough tag for the item
            }
        }

        // If all items are completed and the quest was not already marked as completed
        if (allItemsCompleted && !quest.isCompleted)
        {
            quest.isCompleted = true;
            questText = "<s>" + questText + "</s>"; // Apply strikethrough to the entire quest
        }

        // If the quest was marked as completed earlier, close the strikethrough tag
        if (quest.isCompleted)
        {
            questText += "</s>";
        }

        questListText.text += questText + "\n"; // Append quest text to the quest list UI
    }


    public void ClearQuestUI()
    {
        questListText.text = ""; // Clear the quest list UI when needed
    }
}
