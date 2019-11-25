using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTable : Interactable
{

    public override void InteractionStarts()
    {
        base.InteractionStarts();
        player.LockMovement();
        player.InteractionEvent += StopCrafting;
        GameManager.CraftEvent?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.isCrafting = true;

        
    }


    public void StopCrafting()
    {
        CraftManager.ExitCraftEvent?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.EndCraftEvent?.Invoke();
        player.UnlockMovement();
        ResetInteractable();

        GameManager.isCrafting = false;
    }



}
