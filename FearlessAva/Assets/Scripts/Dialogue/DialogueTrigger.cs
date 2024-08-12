using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string[] speakersDefault;  // Names of speakers before quest is accepted
    [SerializeField] private string[] sentencesDefault; // Sentences spoken before quest is accepted

    [SerializeField] private string[] speakersActive;  // Names of speakers when quest is active but not completed
    [SerializeField] private string[] sentencesActive; // Sentences spoken when quest is active but not completed

    [SerializeField] private string[] speakersCompleted;  // Names of speakers when quest is completed
    [SerializeField] private string[] sentencesCompleted; // Sentences spoken when quest is completed

    [SerializeField] public Quest questToAdd;
    [SerializeField] private GameObject rewardObject; // The reward object to activate upon quest completion

    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerInTrigger = false;
        }
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            if (questToAdd != null)
            {
                if (QuestManager.instance.HasCompletedQuest(questToAdd))
                {
                    // Quest completed dialogues and activate reward
                    DialogueManager.Instance.StartDialogue(speakersCompleted, sentencesCompleted, questToAdd, true);

                    // Activate the reward object
                    if (rewardObject != null)
                    {
                        rewardObject.SetActive(true);
                    }

                    // Remove or mark the quest as completed in the quest manager
                    QuestManager.instance.RemoveQuest(questToAdd);
                }
                else if (QuestManager.instance.HasQuest(questToAdd))
                {
                    // Quest is active but not completed dialogues
                    DialogueManager.Instance.StartDialogue(speakersActive, sentencesActive, questToAdd, false);
                }
                else
                {
                    // Default dialogues before quest is accepted and add the quest
                    DialogueManager.Instance.StartDialogue(speakersDefault, sentencesDefault, questToAdd, true);
                    QuestManager.instance.AddQuest(questToAdd);
                }
            }
        }
    }
}
