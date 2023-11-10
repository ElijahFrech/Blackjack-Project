using UnityEngine;

public class VRMouseMovement : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public float movementSpeed = 5.0f;

    void Update()
    {
        // Rotate the camera based on mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.left, mouseY * rotationSpeed * Time.deltaTime);

        // Move the camera based on keyboard input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
    }
}