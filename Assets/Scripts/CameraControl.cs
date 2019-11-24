using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraControl : MonoBehaviour
{

    public bool active;
    public PlayerInputs input;

    float deltaX;
    float deltaY;

    public float mouseSensitivityX;
    public float mouseSensitivityY;

    public new CinemachineFreeLook camera;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        input = new PlayerInputs();
        input.GamePlay.RotationX.performed += OnRotationX;
        input.GamePlay.RotationY.performed += OnRotationY;

        
    }

    public void OnEnable()
    {
        input.Enable();   
    }

    public void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        camera.m_XAxis.m_InputAxisValue = deltaX*mouseSensitivityX * Time.deltaTime;

        camera.m_YAxis.m_InputAxisValue = deltaY * mouseSensitivityY* Time.deltaTime;

       
    }


    public void OnRotationX(InputAction.CallbackContext context)
    {
        deltaX = context.ReadValue<float>();

        /*if (Mathf.Abs(deltaX) > 10f)
        {
            deltaX = Mathf.Sign(deltaX) * 10f;
        }
        else if (Mathf.Abs(deltaX) <= 1f)
        {
            deltaX = 0f;
        }

        Debug.Log("deltaX: " + deltaX.ToString());*/
    }

    public void OnRotationY(InputAction.CallbackContext context)
    {
        deltaY = context.ReadValue<float>();
        
        /*if(Mathf.Abs(deltaY) > 10f)
        {
            deltaY = Mathf.Sign(deltaY) * 10f;
        }else if(Mathf.Abs(deltaY) <= 1f)
        {
            deltaY = 0f;
        }

        Debug.Log("deltaY: " + deltaY.ToString());*/
    }

}
