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
        textPrompt.text = "Pickup " + data.name;
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
            base.InteractionStarts();

        

        if(player.playerData.storedPickups.Count == 0)
        {
            player.playerData.storedPickups.Add(data);
            player.playerData.amount.Add(1);
        }
        else
        {
            int i = 0;
            bool alreadyHadPickup = false;
            foreach(PickupData storedPickupData in player.playerData.storedPickups)
            {
                if(storedPickupData == data)
                {
                    player.playerData.amount[i]++;
                    alreadyHadPickup = true;
                    break;
                }
                i++;
            }

            if (!alreadyHadPickup)
            {
                player.playerData.storedPickups.Add(data);
                player.playerData.amount.Add(1);
            }
        }
        

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
