using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxProximity : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject InteractUI;
    [SerializeField] private float detectionRange;

    [SerializeField] private DialogueTrigger trigger;
    [SerializeField] private GameObject DialogueUI;
    [SerializeField] private PauseMenu pause;

    void Update()
    {
        if (isPlayerInRange())
        {
            InteractUI.SetActive(true);
            if(Input.GetKeyUp(KeyCode.F))
            {
                pause.toggleUIElements(false);
                DialogueUI.SetActive(true);
                Time.timeScale = 0f;
                //trigger.TriggerDialogue();
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                disableDialogueUI();
            }
        }
        else
        {
            InteractUI.SetActive(false);
        }
    }

    public void disableDialogueUI()
    {
        pause.toggleUIElements(true);
        DialogueUI.SetActive(false);
        Time.timeScale = 1f;
    }

    bool isPlayerInRange()
    {
        if (this != null && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRange)
            {
                return true;
            }
        }
        return false;
    }

}
