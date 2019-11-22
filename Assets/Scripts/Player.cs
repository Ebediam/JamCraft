using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public PlayerInputs input;
    public CharacterController controller;

    public Transform groundCheck;
    public float groundCheckDistance;

    public LayerMask groundLayer;

    bool isGrounded;
    bool jumpBoost;

    public float speed;

    public float gravity = -9.81f;

    Vector3 velocity;

    public float jumpHeight;
    public float jumpBoostAmount;
    public float jumpBoostMaxTime;
    float jumpVelocity;


    Vector2 movementDirection;
    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInputs();
        input.GamePlay.Jump.performed += Jump;
        input.GamePlay.Jump.canceled += JumpBoostEnds;

        input.GamePlay.Move.performed += Move;

        jumpVelocity = Mathf.Sqrt(jumpHeight*gravity*-2f);
        

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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
                     
        if(isGrounded)
        {
            jumpBoost = true;
            if(velocity.y < 0f)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            if (jumpBoost)
            {
                velocity.y += jumpBoostAmount * Time.deltaTime;
            }
        }

        controller.Move(new Vector3(-movementDirection.y, 0f, movementDirection.x)*speed*Time.deltaTime);


        velocity.y += gravity*Time.deltaTime;

        controller.Move(velocity*Time.deltaTime);
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
        if (!isGrounded)
        {
            return;
        }

        
        velocity.y = jumpVelocity;
        jumpBoost = true;
        Invoke("JumpBoostEndInvoke", jumpBoostMaxTime);


    }

    public void JumpBoostEnds(InputAction.CallbackContext context)
    {
        JumpBoostEndInvoke();
    }

    public void JumpBoostEndInvoke()
    {
        CancelInvoke("JumpBoostEndInvoke");
        jumpBoost = false;
    }
}
