using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsRotatingAround : MonoBehaviour
{
    // Start is called before the first frame update
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;
    float rotationSpeed;
    Vector3 rotationDirection;

    void Start(){
         rotationSpeed = Random.Range(5, 35);
      // Randomly choose the direction of rotation around the target
        rotationDirection = (Random.value < 0.5) ? Vector3.up : Vector3.down;


    }

    void Update()
    {
      

       
        // Spin the object around the target at the random speed and direction.
        transform.RotateAround(target.transform.position, rotationDirection, rotationSpeed * Time.deltaTime);

        
  

        // Rotate the object on its own Y-axis at the random speed.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        // Rotate the object on its own X, Y, and Z axes at 20 degrees/second.
         // Rotate the object on its own X, Y, and Z axes at the random speed.
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime);
    
    }
 
}
