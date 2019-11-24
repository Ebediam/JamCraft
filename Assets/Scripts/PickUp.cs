using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PickUp : Interactable
{
    public PickupData data;
    //public DespawnBehaviour despawnBehaviour;


        /*
#if(UNITY_EDITOR)

    private void OnValidate()
    {        
        //return;

        
        if (!despawnBehaviour)
        {
            despawnBehaviour = ScriptableObject.CreateInstance("DespawnBehaviour") as DespawnBehaviour;
            
            
            AssetDatabase.CreateAsset(despawnBehaviour, "Assets/DespawnBehaviour/DespawnBehaviour"+despawnBehaviour.GetHashCode()+".asset");
            AssetDatabase.SaveAssets();
        }
        else
        {
            despawnBehaviour.stayDespawned = false;
        }

    }

#endif
*/
    void Start()
    {
       
        /*if (despawnBehaviour)
        {
            if (despawnBehaviour.stayDespawned)
            {
                gameObject.SetActive(false);
            }
        }*/

        
    }


    public override void InteractionBubbleStarts()
    {
        base.InteractionBubbleStarts();
        textPrompt.text = data.verb+" "+ data.name;
    }

    public override void InteractionBubbleEnds()
    {
        base.InteractionBubbleEnds();
    }


    public override void InteractionStarts()
    {

        if (!player)
        {
            Debug.Log("No player detected");
            return;
        }

        bool receiveItem = false;
        if (data.requiredTool)
        {
            foreach(PickupData item in player.playerData.storedPickups)
            {
                if(data.requiredTool == item)
                {
                    receiveItem = true;
                                        
                }
            }
        }
        else
        {
            receiveItem = true;
        }

        if (!receiveItem)
        {
            return;
        }


        base.InteractionStarts();



        GameManager.GiveItemToPlayer(player, data);


        ResetInteractable();

        /*if (despawnBehaviour)
        {
            despawnBehaviour.stayDespawned = true;

        }*/

        
        this.gameObject.SetActive(false); 
        

    }

    public override void InteractionEnds()
    {
        base.InteractionEnds();
    }


}
