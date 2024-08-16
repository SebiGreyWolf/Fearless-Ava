using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] speakers;
    public string[] questStartSentences; // Sentences when the quest is first taken
    public string[] questActiveSentences; // Sentences when the quest is active but not completed
    public string[] questCompleteSentences; // Sentences when the quest is completed
    public Quest quest;
    //Working but not with style
    public GameObject questReward;

    private int currentSpeakerIndex = 0;

    private void Start()
    {
        if (quest != null)
            quest.ResetQuestState();

        if (questReward != null)
        {
            quest.reward = questReward;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && FindObjectOfType<QuestManager>().ContainsQuest(quest) && Input.GetKeyDown(KeyCode.F))
            FindObjectOfType<DialogueManager>().StartDialogue(this);
    }
    public void ActivateQuest()
    {
        if (quest != null)
            FindObjectOfType<QuestManager>().ActivateQuest(quest);
    }
    public bool RemoveQuest()
    {
        if (quest.isCompleted && quest != null)
        {
            FindObjectOfType<QuestManager>().RemoveQuest(quest);
            FindObjectOfType<QuestManager>().ActivateReward(quest);
        }
        return quest.isCompleted;
    }
    public string GetCurrentSpeakerName()
    {
        string currentSpeaker = speakers[currentSpeakerIndex];
        currentSpeakerIndex++;  // Move to the next speaker index

        // If the speaker index exceeds the number of available speakers, reset to the first speaker
        if (currentSpeakerIndex >= speakers.Length)
        {
            currentSpeakerIndex = 0;
        }
        return currentSpeaker;
    }

    public string[] GetCurrentSentences()
    {
        if (quest.isCompleted)
        {
            return FormatSentences(questCompleteSentences); // Sentences for a completed quest
        }
        else if (quest.isActive)
        {
            return FormatSentences(questActiveSentences); // Sentences for an active but incomplete quest
        }
        else
        {
            return FormatSentences(questStartSentences); // Sentences for when the quest is being activated
        }
    }

    private string[] FormatSentences(string[] sentences)
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            sentences[i] = sentences[i].Replace("[b]", "<b>").Replace("[/b]", "</b>");
            sentences[i] = sentences[i].Replace("[i]", "<i>").Replace("[/i]", "</i>");
        }
        return sentences;
    }
}
