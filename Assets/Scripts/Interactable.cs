using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public bool interactableCamera;
    public CinemachineVirtualCamera interactCam;
    public Player player;
    public TextMeshPro textPrompt;
    public Transform textPromptMover;
    private LookAt lookAt;
    bool isInteracting;
    


    public void Awake()
    {
        textPrompt.gameObject.SetActive(false);
        lookAt = textPromptMover.gameObject.AddComponent<LookAt>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            lookAt.player = player;
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
        InteractionBubbleEnds();
        if (interactableCamera)
        {
            interactCam.Priority = 15;
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
        if (isInteracting)
        {
            InteractionEnds();

        }
    }

}
