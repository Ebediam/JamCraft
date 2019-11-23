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

    // Update is called once per frame
    void Update()
    {
        
    }
}
