using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxProximity : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject textBox;
    [SerializeField] private float detectionRange;
    [SerializeField] private AudioSource g;

    private void Start()
    {
        g.Play();
        g.Pause();
    }

    void Update()
    {
        if (isPlayerInRange())
        {
            textBox.SetActive(true);
            g.UnPause();
        }
        else
        {
            textBox.SetActive(false);
            g.Pause();
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
