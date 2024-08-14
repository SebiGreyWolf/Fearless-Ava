using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;
    public GameObject questUI;
    public Text questListText; // Reference to the UI Text component that displays the quest list
    private Queue<string> sentences;

    private DialogueTrigger currentDialogueTrigger;
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger)
    {
        dialogueBox.SetActive(true); // Activate the dialogue box
        currentDialogueTrigger = dialogueTrigger;

        nameText.text = dialogueTrigger.GetCurrentSpeakerName();
        sentences.Clear();

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

        string questText = "";

        if (quest.isCompleted)
        {
            questText += "<s>"; // Start strikethrough tag
        }

        questText += $"<b>{quest.questName}</b>:\n"; // Quest name in bold

        foreach (Item item in quest.requiredItems)
        {
            questText += $"{item.name}: {item.currentCount}/{item.maxCount}\n"; // Item name and amount
        }

        if (quest.isCompleted)
        {
            questText += "</s>"; // End strikethrough tag
        }

        questListText.text += questText + "\n"; // Append quest text to the quest list UI
    }

    public void ClearQuestUI()
    {
        questListText.text = ""; // Clear the quest list UI when needed
    }
}
