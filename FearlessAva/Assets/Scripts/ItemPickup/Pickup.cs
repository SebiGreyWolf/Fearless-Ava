using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    public GameObject canvers;

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

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (canvers != null)
                canvers.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                Inventory.instance.AddItem(item);
                Destroy(gameObject);

                if (canvers != null)
                    canvers.SetActive(false);
            }
        }
        else
        {
            if (canvers != null)
                canvers.SetActive(false);
        }
    }
}
