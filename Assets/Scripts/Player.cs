using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class Player : MonoBehaviour
{
    public PlayerData playerData;

    public Animator animator;

    public delegate void InteractionDelegate();
    public InteractionDelegate InteractionEvent;

    public PlayerInputs input;
    public CharacterController controller;

    public Transform groundCheck;
    public Transform ceilingCheck;
    public float groundCheckDistance;

    public LayerMask groundLayer;

    public List<AudioSource> stepSFX;

    bool isGrounded;
    bool jumpBoost;
    bool lockMovement;

    public float speed;

    public bool thirdPersonControl;

    float turnSmoothVelocity;
    public float gravity = -9.81f;
    public bool ignoreGravity;

    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float currentSpeed;

    public float jumpHeight;
    public float jumpBoostAmount;
    public float jumpBoostMaxTime;
    float jumpVelocity;

    bool isTalking;
    public Transform cameraTransform;

    public AudioSource jumpSFX;
    public AudioSource fallSFX;

    Vector2 movementDirection;
    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInputs();
        input.GamePlay.Jump.performed += InteractionOrJump;
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
        if (GameManager.isPaused)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);

        if(Physics.CheckSphere(ceilingCheck.position, groundCheckDistance, groundLayer))
        {
            jumpBoost = false;
            velocity.y = 0f;
        }
                     
        if(isGrounded)
        {
            if (!jumpBoost)
            {
                if (!fallSFX.isPlaying)
                {
                    fallSFX.Play();
                }
                
            }
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

        if (!lockMovement)
        {
            if (thirdPersonControl)
            {
                
                if (movementDirection.y != 0)
                {
                    
                    currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.1f);

                    float targetRotation = Mathf.Atan2(movementDirection.x, movementDirection.y) * Mathf.Rad2Deg +cameraTransform.eulerAngles.y ;

                    transform.localEulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                    PlayFootstepSFX();

                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, 0f, 0.2f);
                }                              
                

                controller.Move(transform.forward * currentSpeed*movementDirection.y*Time.deltaTime);
            }
            else
            {
                controller.Move(new Vector3(-movementDirection.y, 0f, movementDirection.x) * speed * Time.deltaTime);

                transform.LookAt(transform.position + new Vector3(-movementDirection.y, 0f, movementDirection.x));
            }

        }
        else
        {
            currentSpeed = 0f;
        }




        if (!ignoreGravity)
        {
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }




        animator.SetFloat("speedPercent", currentSpeed/speed);
        animator.SetBool("isJumping", !isGrounded);

    }

    public void Move(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();

    }

    public void CameraMove(InputAction.CallbackContext context)
    {

    }

    public void InteractionOrJump(InputAction.CallbackContext context)
    {
        if (!isGrounded)
        {
            return;
        }

        if (InteractionEvent != null)
        {
            InteractionEvent();
        }
        else
        {            
            Jump();
        }    

        
    }

    public void Jump()
    {
        jumpSFX.Play();
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

    public void LockMovement()
    {
        if (lockMovement)
        {
            return;
        }
        lockMovement = true;
    }

    public void UnlockMovement()
    {
        if (!lockMovement)
        {
            return;
        }

        lockMovement = false;
    }

    public void PlayFootstepSFX()
    {

        foreach(AudioSource audio in stepSFX)
        {
            if (audio.isPlaying)
            {
                return;
            }
        }

        int index = Random.Range(0, stepSFX.Count);
        stepSFX[index].Play();

    }


}
