using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Interactable
{
    public Transform startPoint;
    public Transform endPoint;
    public float climbSpeed;

    bool isClimbing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void InteractionStarts()
    {
        base.InteractionStarts();
        player.LockMovement();
        player.controller.enabled = false;
        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;
        player.controller.enabled = true;
        isClimbing = true;
        player.ignoreGravity = true;
        interactCam.m_Follow = player.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isClimbing)
        {
            isClimbing = false;
            player.controller.enabled = false;
            player.transform.position = endPoint.position;
            player.transform.rotation = endPoint.rotation;
            player.controller.enabled = true;
            player.UnlockMovement();
            player.ignoreGravity = false;
            

        }
        ResetInteractable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClimbing)
        {
            player.controller.Move(Vector3.up * climbSpeed * Time.deltaTime);
        }
    }
}
