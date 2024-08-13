using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxProximity : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject InteractUI;
    [SerializeField] private float detectionRange;

    void Update()
    {
        if (isPlayerInRange())
        {
            InteractUI.SetActive(true);
        }
        else
        {
            InteractUI.SetActive(false);
        }
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
