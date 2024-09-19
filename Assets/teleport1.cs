using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class teleport1 : MonoBehaviour
{
    [SerializeField]
    private Transform teleportDestination; // Destination to teleport the player

    [SerializeField]
    private TextMeshPro actionText; // TextMeshPro UI element for interaction message

    private bool isPlayerInRange = false; // To track if the player is in range for interaction

    private void Start()
    {
        if (actionText != null)
        {
            actionText.gameObject.SetActive(false); // Hide the text at the start
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (actionText != null)
            {
                actionText.gameObject.SetActive(true); // Show interaction text
                actionText.text = "Press F to use elevator"; // Set the message
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (actionText != null)
            {
                actionText.gameObject.SetActive(false); // Hide the text when player leaves
            }
        }
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && teleportDestination != null)
        {
            player.transform.position = teleportDestination.position; // Teleport the player
        }
    }

    // Provide the destination for teleportation
    public Transform GetDestination()
    {
        return teleportDestination;
    }
}
