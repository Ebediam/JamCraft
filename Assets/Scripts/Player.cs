using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public PlayerInputs input;
    public CharacterController controller;
    public new Camera camera;


    public float speed;


    public Vector2 movementDirection;
    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInputs();
        input.GamePlay.Jump.performed += Jump;

        input.GamePlay.Move.performed += Move;
        

    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        controller.Move(new Vector3(-movementDirection.y, 0f, movementDirection.x)*speed*Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();

    }

    public void CameraMove(InputAction.CallbackContext context)
    {

    }

    public void Jump(InputAction.CallbackContext context)
    {
        transform.localScale *= 1.1f;
    }
}
