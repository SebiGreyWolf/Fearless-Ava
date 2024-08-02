using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxProximity : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject textBox;
    [SerializeField] private float detectionRange;

    private void Start()
    {
        FindObjectOfType<AudioManagement>().PlaySound("ProximityBoxTest");
        FindObjectOfType<AudioManagement>().PauseSound("ProximityBoxTest");
    }

    void Update()
    {
        if (isPlayerInRange())
        {
            textBox.SetActive(true);
            FindObjectOfType<AudioManagement>().UnPauseSound("ProximityBoxTest");
        }
        else
        {
            textBox.SetActive(false);
            FindObjectOfType<AudioManagement>().PauseSound("ProximityBoxTest");
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
