using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoQuestDialogueTrigger : MonoBehaviour
{
    public string[] speakers;
    public string[] sentences;
    public Quest requrementQuest;
    public GameObject reward;

    private DialogueManager dialogueManager;
    private int currentSpeakerIndex;
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && Input.GetKeyDown(KeyCode.F))
        {
            if (requrementQuest.isCompleted)
            {
                dialogueManager.StartNoQuestDialogue(this,sentences);
            }
        }
    }
    public string GetCurrentSpeakerName()
    {
        string currentSpeaker = speakers[currentSpeakerIndex];
        currentSpeakerIndex++;
        if (currentSpeakerIndex >= speakers.Length)
        {
            currentSpeakerIndex = 0;
        }
        return currentSpeaker;
    }

    public void ActivateReward()
    {
        FindObjectOfType<AudioManagement>().PlaySound("QuestReward");
        reward.SetActive(true);
    }
}
