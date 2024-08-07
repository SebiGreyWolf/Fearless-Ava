using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    public GameObject canvers;

    private void OnTriggerStay2D(Collider2D other)
    {
        canvers.SetActive(true);
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvers.SetActive(false);
    }
}
