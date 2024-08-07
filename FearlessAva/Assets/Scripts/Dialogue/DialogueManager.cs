using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public Text NPCName;
    public Text dialogueText;

    private Queue<string> sentences;

    [SerializeField] private GameObject DialogueUI;
    [SerializeField] private PauseMenu pause;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if(DialogueUI.activeSelf && Input.GetKeyUp(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        NPCName.text = dialogue.NPCName;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence (string sentence)
    {
        string[] subs = sentence.Split(' ', 2);
        NPCName.text = subs[0];
        dialogueText.text = "";
        foreach (char letter in subs[1].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
        pause.toggleUIElements(true);
        DialogueUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
