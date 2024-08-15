using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item; // The item this pickup represents
    public GameObject canvas; // UI element to show when player is in range

    private bool isPlayerInTrigger = false;
    private Inventory inventory;
    private QuestManager manager;

    private void Start()
    {
        // Find the GameManager and get the Inventory component
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            inventory = gameManager.GetComponent<Inventory>();
            manager = gameManager.GetComponent<QuestManager>();
        }
        else
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

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
            if (canvas != null)
                canvas.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupItem();
            }
        }
        else
        {
            if (canvas != null)
                canvas.SetActive(false);
        }
    }

    // Method to handle item pickup and adding it to the inventory
    public void PickupItem()
    {

        if (inventory != null && manager.CanPickup(item))
        {
            inventory.AddItem(item);
            FindObjectOfType<AudioManagement>().PlaySound("PickUp");   
            Destroy(gameObject);  // Destroy the pickup item after it's collected
            if (canvas != null)
                canvas.SetActive(false);
        }
    }

    // Call this method when the enemy is defeated to auto-pickup the item
    public void AutoPickup()
    {
        if (item != null)
        {
            PickupItem();
        }
    }
}
