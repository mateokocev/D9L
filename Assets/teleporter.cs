using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Transform teleportDestination; // Destination to teleport the player

    [SerializeField]
    private TextMeshPro actionText; // TextMeshPro UI element for interaction message

    private void Start()
    {
        if (actionText != null)
        {
            actionText.gameObject.SetActive(false); // Hide the text at the start
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (actionText != null)
            {
                actionText.gameObject.SetActive(true); // Show interaction text
                actionText.text = "Press F to Teleport"; // Set the message
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (actionText != null)
            {
                actionText.gameObject.SetActive(false); // Hide the text when player leaves
            }
        }
    }

    // Provide the destination for teleportation
    public Transform GetDestination()
    {
        return teleportDestination;
    }
}
