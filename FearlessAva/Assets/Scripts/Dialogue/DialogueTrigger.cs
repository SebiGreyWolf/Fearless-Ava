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

    private int currentSentenceIndex = 0;
    private bool questActivated = false;

    private void Start()
    {
        if (quest != null)
            quest.ResetQuestState();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && Input.GetKeyDown(KeyCode.F))
        {
            FindObjectOfType<DialogueManager>().StartDialogue(this);
        }
    }
    public void ActivateQuest()
    {
        if (!questActivated && quest != null)
        {
            FindObjectOfType<QuestManager>().ActivateQuest(quest);
            questActivated = true;
        }
    }
    public bool RemoveQuest()
    {
        if (quest.isCompleted && quest != null)
        {
            FindObjectOfType<QuestManager>().RemoveQuest(quest);
            questActivated = false;
        }
        return quest.isCompleted;
    }

    public string GetCurrentSpeakerName()
    {
        return speakers[currentSentenceIndex % speakers.Length];
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
