using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] speakers;
    public List<QuestDialogue> questDialogues; // List of all possible dialogues related to quests
    public List<NoQuestDialogue> basicDialogues;
    public GameObject rewardObject; // The reward object to activate upon quest completion (optional)

    private QuestManager questManager;
    private int currentSpeakerIndex;

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && Input.GetKeyDown(KeyCode.F))
        {
            // Check if there is an active quest that is not completed
            var activeQuest = questManager.allQuests.Find(q => q.isActive && !q.isCompleted);

            if (activeQuest != null)
            {
                // Start the quest dialogue if an active quest is found
                StartDialogue(activeQuest);
            }
            else
            {
                // No active quest, check if there are any available quests
                bool questDialogueStarted = CheckAvailableQuests();

                // If no available quests were found, trigger no-quest dialogues
                if (!questDialogueStarted)
                {
                    CheckNoQuestDialogues();
                }
            }
        }
    }
    private void StartDialogue(Quest activeQuest)
    {
        var dialogueManager = FindObjectOfType<DialogueManager>();
        var sentences = GetDialogueForQuest(activeQuest);
        if (sentences != null)
        {
            dialogueManager.StartDialogue(this, activeQuest);
        }
    }

    private bool CheckAvailableQuests()
    {
        foreach (var questDialogue in questDialogues)
        {
            if (questDialogue.requirement == null || questDialogue.requirement.isCompleted)
            {
                if (questManager.ContainsQuest(questDialogue.currentQuest))
                {
                    StartDialogue(questDialogue.currentQuest);
                    return true;
                }
            }
        }
        return false;
    }
    private void CheckNoQuestDialogues()
    {
        foreach (var noQuestDialogue in basicDialogues)
        {
            if (noQuestDialogue.requrementQuest == null || noQuestDialogue.requrementQuest.isCompleted)
            {
                StartNoQuestDialogue(noQuestDialogue);
                return;
            }
        }
    }
    private void StartNoQuestDialogue(NoQuestDialogue noQuestDialogue)
    {
        var dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartNoQuestDialogue(this, noQuestDialogue.sentences);
    }

    public string GetCurrentSpeakerName()
    {
        string currentSpeaker = speakers[currentSpeakerIndex];
        Debug.Log(currentSpeaker);
        currentSpeakerIndex++;
        if (currentSpeakerIndex >= speakers.Length)
        {
            currentSpeakerIndex = 0;
        }
        return currentSpeaker;
    }

    public string[] GetDialogueForQuest(Quest currentQuest)
    {
        // Find the relevant QuestDialogue based on the current quest
        var dialogue = questDialogues.Find(q => q.currentQuest == currentQuest);
        if (dialogue == null) return null;

        if (!currentQuest.isActive)
        {
            return dialogue.questStartSentences;
        }
        else if (!currentQuest.isCompleted)
        {
            return dialogue.questActiveSentences;
        }
        else
        {
            return dialogue.questCompleteSentences;
        }
    }

    public string[] FormatSentences(string[] sentences)
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            sentences[i] = sentences[i].Replace("[b]", "<b>").Replace("[/b]", "</b>");
            sentences[i] = sentences[i].Replace("[i]", "<i>").Replace("[/i]", "</i>");
        }
        return sentences;
    }

    private bool CheckForActivatingReward()
    {
        foreach (var questDialogue in questDialogues)
        {
            if (!questDialogue.currentQuest.isCompleted)
                return false;
        }

        if (basicDialogues != null || basicDialogues.Count > 0)
            if (!basicDialogues[0].requrementQuest.isCompleted || basicDialogues[0].reward == null)
                return false;

        return true;
    }
    public void HandleReward()
    {
        if (rewardObject != null)
        {
            if (CheckForActivatingReward())
            {
                Debug.Log("Activate Quest");
                rewardObject.SetActive(!rewardObject.activeInHierarchy); // Activates the reward object when quest is completed
                FindObjectOfType<AudioManagement>().PlaySound("QuestReward");
            }

        }
    }
    public void RemoveDialogue()
    {
        basicDialogues.RemoveAt(0);
    }
}