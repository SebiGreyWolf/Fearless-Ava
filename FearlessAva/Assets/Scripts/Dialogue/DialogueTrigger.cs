using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string[] speakers;  // Names of speakers in the conversation
    [SerializeField] private string[] sentences; // Sentences spoken by the speakers in order
    [SerializeField] public Quest questToAdd;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("In here here");
        if (collision.gameObject.GetComponent<Player>() && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Got in here");
            DialogueManager.Instance.StartDialogue(speakers, sentences, questToAdd);
        }
    }
}
