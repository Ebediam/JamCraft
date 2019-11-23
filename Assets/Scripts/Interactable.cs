﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public bool interactableCamera;
    public CinemachineVirtualCamera interactCam;
    protected Player player;
    public TextMeshPro textPrompt;

    bool isInteracting;

    public void Awake()
    {
        textPrompt.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Debug.Log("OnTriggerenter");
            player = other.GetComponent<Player>();
            player.InteractionEvent += InteractionStartChecker;
            InteractionBubbleStarts();
        }
    }
       
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            ResetInteractable();           
        }
    }


    public virtual void InteractionStartChecker()
    {
        if (!isInteracting)
        {
            InteractionStarts();
        }
    }

    public virtual void InteractionStarts()
    {
        isInteracting = true;
        if (interactableCamera)
        {
            interactCam.Priority = 11;
        }
    }

    public virtual void InteractionEnds()
    {
        isInteracting = false;
        if (interactableCamera)
        {
            interactCam.Priority = 1;
        }
    }

    public virtual void InteractionBubbleStarts()
    {
        textPrompt.gameObject.SetActive(true);
    }

    public virtual void InteractionBubbleEnds()
    {
        textPrompt.gameObject.SetActive(false);
    }

    public void ResetInteractable()
    {
        player.InteractionEvent = null;
        player = null;
        InteractionBubbleEnds();
    }

}
