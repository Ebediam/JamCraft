using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathZone : MonoBehaviour
{

    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Player player = other.GetComponent<Player>();
            player.controller.enabled = false;
            player.transform.position = spawnPoint.position;
            player.controller.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
