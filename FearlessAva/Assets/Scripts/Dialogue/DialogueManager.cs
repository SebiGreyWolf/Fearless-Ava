using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;         // UI text to display the speaker's name
    public Text dialogueText;     // UI text to display the dialogue sentence
    public GameObject dialogue;
    

    private List<string> speakersList; // Queue to manage the order of speakers
    private Queue<string> sentencesQueue; // Queue to manage the order of sentences
    private int currentSpeakerIndex;
    private Quest quest;

    public static DialogueManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern to ensure only one instance of DialogueManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        speakersList = new List<string>();
        sentencesQueue = new Queue<string>();
    }

    public void StartDialogue(string[] speakers, string[] sentences, Quest addedQuest)
    {
        speakersList.Clear();
        sentencesQueue.Clear();

        speakersList.AddRange(speakers);

        dialogue.SetActive(true);
        quest = addedQuest;

        foreach (string sentence in sentences)
        {
            sentencesQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    // Display the next sentence in the dialogue
    public void DisplayNextSentence()
    {
        if (sentencesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Rotate through the speakers
        string speaker = speakersList[currentSpeakerIndex];
        currentSpeakerIndex = (currentSpeakerIndex + 1) % speakersList.Count;

        string sentence = sentencesQueue.Dequeue();

        nameText.text = speaker;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // Coroutine to type out the sentence letter by letter
    IEnumerator TypeSentence(string sentence)
    {
        string[] subs = sentence.Split(' ', 2);
        //NPCName.text = subs[0];
        dialogueText.text = "";
        foreach (char letter in subs[1].ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    // End the dialogue
    void EndDialogue()
    {
        dialogue.SetActive(false);
        if (quest != null)
        {
            QuestManager.instance.AddQuest(quest);
            QuestManager.instance.UpdateQuestUI();  // Assuming QuestManager has this method to update the UI
        }
    }
}
