using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Teleport2 : MonoBehaviour
{
    [SerializeField]
    private Transform[] teleportDestinations; // Array of destinations to teleport the player

    [SerializeField]
    private TextMeshPro actionText; // TextMeshPro UI element for interaction message

    private int currentDestinationIndex = 0; // Track the current teleport destination

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
                actionText.text = "Press F to teleport"; // Set the message
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

    // Provide the next destination for teleportation
    public Transform GetDestination()
    {
        if (teleportDestinations.Length == 0) return null;

        Transform destination = teleportDestinations[currentDestinationIndex];
        currentDestinationIndex = (currentDestinationIndex + 1) % teleportDestinations.Length; // Cycle through destinations
        return destination;
    }
}
