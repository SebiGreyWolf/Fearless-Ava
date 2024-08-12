using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    public GameObject canvers;

    private bool isPlayerInTrigger = false;

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerInTrigger = true;
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerInTrigger = false;
        }
    }

    // Update the canvas visibility and handle item pickup
    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (canvers != null)
                canvers.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupItem();
            }
        }
        else
        {
            if (canvers != null)
                canvers.SetActive(false);
        }
    }

    // Method to handle item pickup and adding it to the inventory
    public void PickupItem()
    {
        Inventory.instance.AddItem(item);
        Destroy(gameObject);  // Destroy the pickup item after it's collected
        if (canvers != null)
            canvers.SetActive(false);
    }

    // Call this method when the enemy is defeated to auto-pickup the item
    public void AutoPickup()
    {
        if (item != null)
        {
            PickupItem();
        }
    }

    // Optional: If you want the item to automatically be picked up when the enemy is destroyed
    private void OnDestroy()
    {
        if (!isPlayerInTrigger)
        {
            AutoPickup();
        }
    }
}
