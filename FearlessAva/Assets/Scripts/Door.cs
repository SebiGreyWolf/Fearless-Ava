using Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Reference to the other door to teleport to
    public Door connectedDoor;

    public Collider2D newCameraBounds;

    private bool isPlayerInTrigger = false;
    public CinemachineConfiner2D confiner;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && Input.GetKeyDown(KeyCode.F))
        {
            // Teleport the player to the connected door
            TeleportPlayer(collision.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        if (connectedDoor != null)
        {
            // Move the player to the position of the connected door
            player.transform.position = connectedDoor.transform.position;


            Debug.Log(confiner);
            Debug.Log(newCameraBounds);
            // Update the camera confiner's bounding shape
            if (confiner != null && newCameraBounds != null)
            {
                Debug.Log("Get in here");
                confiner.m_BoundingShape2D = newCameraBounds;
            }
        }
        else
        {
            Debug.LogWarning("No connected door assigned!");
        }
    }
}
