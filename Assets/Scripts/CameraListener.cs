using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraListener : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;
    public Player player;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            camera.Priority = 11;
            camera.LookAt = player.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {            
            camera.Priority = 1;
            player = null;
            camera.LookAt = null;
        }
    }

}
