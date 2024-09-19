using UnityEngine;

public class destroyCar : MonoBehaviour
{
    // This function is called when another collider enters this GameObject's collider
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("test");
        // Check if the object has the tag "Car"
        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log("testt");
            // Destroy the GameObject with the "Car" tag
            Destroy(collision.gameObject);
        }
    }
}

