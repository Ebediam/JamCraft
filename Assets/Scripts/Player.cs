using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class Player : MonoBehaviour
{
    public PlayerData playerData;

    public Animator animator;

    public delegate void PlayerEventDelegate();
    public PlayerEventDelegate InteractionEvent;
    public static PlayerEventDelegate UseToolEvent;

    public delegate void EquipDelegate(PickupData item);
    public static EquipDelegate EquipEvent;
    public static PlayerEventDelegate UnequipEvent;
    
    

    public PlayerInputs input;
    public CharacterController controller;

    public Transform groundCheck;
    public Transform ceilingCheck;
    public float groundCheckDistance;

    public LayerMask groundLayer;

    public AudioSource stepSFX;

    public ObjectHolder objectHolder;
    public GameObject holdedObject;
    public PickupData holdedObjectData;
    public bool isHandling;

    bool isGrounded;
    bool jumpBoost;
    bool lockMovement;

    bool playingFootstepSFX;
    public float speed;

    public bool thirdPersonControl;

    float turnSmoothVelocity;
    public float gravity = -9.81f;
    public bool ignoreGravity;

    public float stepRate;

    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float currentSpeed;

    public float jumpHeight;
    public float jumpBoostAmount;
    public float jumpBoostMaxTime;
    float jumpVelocity;

    bool isTalking;
    public bool isRunning;
    public Transform cameraTransform;

    public static Player local;

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

        EquipEvent += EquipTool;
        UnequipEvent += UnequipTool;
        UseToolEvent += PlayUseAnimation;

    }

    private void Start()
    {
        local = FindObjectOfType<Player>();
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

        /*if(Physics.CheckSphere(ceilingCheck.position, groundCheckDistance, groundLayer))
        {
            jumpBoost = false;
            velocity.y = 0f;
        }*/

        
                     
        if(isGrounded)
        {
            if (!jumpBoost)
            {
                if (!fallSFX.isPlaying)
                {
                    fallSFX.Play();                    
                    isRunning = false;
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
                //Third person
                float targetRotation = Mathf.Atan2(movementDirection.x, movementDirection.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;                              
                              
                if (movementDirection.y != 0)
                {
                    transform.localEulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                    currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.1f);
                    controller.Move((transform.forward * Mathf.Sign(movementDirection.y)) * currentSpeed * movementDirection.y * Time.deltaTime);
                    

                    isRunning = true;
                    

                }
                else if(movementDirection.x != 0)
                {
                    transform.localEulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                    currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.1f);
                    isRunning = true;

                    controller.Move((transform.forward * Mathf.Sign(movementDirection.x)) * currentSpeed * movementDirection.x * Time.deltaTime);
                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, 0f, 0.2f);
                    isRunning = false;
                }                           
                

                
            }
            else
            {
               if(movementDirection.y != 0f)
                {
                    isRunning = true;
                    currentSpeed = speed;
                }
                else
                {
                    isRunning = false;
                    currentSpeed = 0f;
                }

                //Eagles eye
                
                controller.Move(new Vector3(-movementDirection.y, 0f, movementDirection.x) * speed * Time.deltaTime);
                
                transform.LookAt(transform.position + new Vector3(-movementDirection.y, 0f, movementDirection.x));
            }

        }
        else
        {
            currentSpeed = 0f;
            isRunning = false;
        }




        if (!ignoreGravity)
        {
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }


        

        animator.SetFloat("speedPercent", currentSpeed/speed);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isFalling", !jumpBoost);

        if(isRunning && !playingFootstepSFX && isGrounded)
        {
            playingFootstepSFX = true;
            InvokeRepeating("PlayFootstepSFX", 0f, stepRate);
        }

        if (!isGrounded || !isRunning)
        {
            CancelInvoke("PlayFootstepSFX");
            playingFootstepSFX = false;
        }

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
        else if(!lockMovement)
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
        
        stepSFX.Play();
    }

    public void EquipTool(PickupData toolData)
    {
        UnequipTool();

        holdedObject = Instantiate(toolData.toolPrefab);
        holdedObject.transform.position = objectHolder.transform.position;
        holdedObject.transform.rotation = objectHolder.transform.rotation;
        holdedObject.transform.parent = objectHolder.transform;
        holdedObjectData = toolData;
        isHandling = true;
    }

    public void UnequipTool()
    {
        if (holdedObject)
        {
            Destroy(holdedObject);
            holdedObject = null;
            isHandling = false;
        }
    }

    public void PlayUseAnimation()
    {
        animator.Play("Use");
    }


}
