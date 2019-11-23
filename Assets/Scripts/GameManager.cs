using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if(player.playerData.activeLevel == spawnPoint.sceneYouComeFrom)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                break;
            }
        }

        player.playerData.activeLevel = SceneManager.GetActiveScene().buildIndex;
    }


    public static void GiveItemToPlayer(Player player, PickupData item)
    {
        if (player.playerData.storedPickups.Count == 0)
        {
            player.playerData.storedPickups.Add(item);
            player.playerData.amount.Add(1);
        }
        else
        {
            int i = 0;
            bool alreadyHadPickup = false;
            foreach (PickupData storedPickupData in player.playerData.storedPickups)
            {
                if (storedPickupData == item)
                {
                    player.playerData.amount[i]++;
                    alreadyHadPickup = true;
                    break;
                }
                i++;
            }

            if (!alreadyHadPickup)
            {
                player.playerData.storedPickups.Add(item);
                player.playerData.amount.Add(1);
            }
        }
    }

}
