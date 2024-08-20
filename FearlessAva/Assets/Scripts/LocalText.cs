using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalText : MonoBehaviour
{
    public Player player;
    public GameObject text;
    private bool isInArea;

    void Update()
    {
        if(isInArea)
        {
            text.SetActive(true);
        }
        else
        {
            text.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isInArea = false;
        }
    }


}
