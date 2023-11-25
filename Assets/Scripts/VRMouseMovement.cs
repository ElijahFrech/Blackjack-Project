using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRMouseMovement : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public float movementSpeed = 5.0f;

    [SerializeField] public CardManager cardManager;

    // Add a public UnityEvent that can be set in the Unity Editor

    public InputActionProperty leftActivate;
    public XRController leftController; // Add this line

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

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                //Debug.Log("Clicked on: " + hit.transform.name);
                string name = hit.transform.name;

                if (name == "Interactable")
                {
                    cardManager.DealerHit();
                    //cardManager.DealCards();
                    //cardManager.DealerCards();
                }
                else if (name == "Interactable2")
                {
                    cardManager.rotateDealerCard();
                    GameManager.state = GameManager.GameState.DealerTurn;
                    //cardManager.DealersTurn();
                    //cardManager.DealerCards();
                }
                else if (name == "Interactable3")
                {
                    cardManager.PlayerHit();
                }
                else if (name == "Interactable4")
                {
                    cardManager.makeBetButton();
                }


            }


        }



    }
}