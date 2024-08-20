using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestDialogue
{
    public Quest currentQuest;
    public string[] questStartSentences; // Sentences when the quest is first taken
    public string[] questActiveSentences; // Sentences when the quest is active but not completed
    public string[] questCompleteSentences;
    public Quest requirement;
}
