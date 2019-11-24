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
    }

    public void OnRotationY(InputAction.CallbackContext context)
    {
        deltaY = context.ReadValue<float>();
    }

}
