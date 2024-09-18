using UnityEngine;

public class carmovment : MonoBehaviour
{
    // Public variables to control the car's movement speed
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Get input for forward/backward movement (W/S or Up/Down arrow keys)
        float moveVertical = Input.GetAxis("Vertical"); // W/S keys (or arrow keys)

        // Get input for left/right movement (A/D or Left/Right arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys (or arrow keys)

        // Move the car forward/backward based on input
        transform.Translate(Vector3.up * moveVertical * moveSpeed * Time.deltaTime);

        // Move the car left/right based on input
        transform.Translate(Vector3.right * moveHorizontal * moveSpeed * Time.deltaTime);
    }
}
