using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonControllerScript : MonoBehaviour
{
    float movementSpeed = 5;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (Mouse.current.leftButton.isPressed){
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }
    }
}
