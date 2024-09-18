using UnityEngine;
using UnityEngine.SceneManagement;  // Needed for scene management

public class CarBossScript : MonoBehaviour
{
    public bool moveToTarget = true;  // Control whether to move or not
    public Vector2 targetPosition = new Vector2(768, 730);  // Target coordinates (x = 768, y = 730)
    public float moveSpeed = 5f;  // Speed at which the car will move towards the target
    public float flickForce = 500f;  // The strength of the flick
    public float sceneChangeDelay = 5f;  // Delay before changing the scene

    private Rigidbody2D rb;  // Reference to the Rigidbody2D component

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Disable the Rigidbody2D at the start
        if (rb != null)
        {
            rb.isKinematic = true;  // Disable Rigidbody2D movement and physics interactions
        }
    }

    void Update()
    {
        if (moveToTarget)
        {
            // Calculate the current position and target position in 3D space
            Vector3 targetPos3D = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

            // Move the car towards the target position at the specified speed
            transform.position = Vector3.MoveTowards(transform.position, targetPos3D, moveSpeed * Time.deltaTime);
        }
    }

    // Detect collision with the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object that collided with the car has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enable the Rigidbody2D
            if (rb != null)
            {
                rb.isKinematic = false;  // Enable Rigidbody2D physics
            }

            // Disable the movement to the target position
            moveToTarget = false;

            // Apply a "flick" force in a random direction
            Vector2 flickDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;  // Random upward direction
            rb.AddForce(flickDirection * flickForce, ForceMode2D.Impulse);  // Apply impulse force

            // Invoke the scene change after a delay
            Invoke("ChangeScene", sceneChangeDelay);
        }
    }

    // Function to change the scene
    void ChangeScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene (scene index + 1)
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
