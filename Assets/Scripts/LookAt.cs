using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Player player;
    public float height = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            transform.LookAt(player.cameraTransform);
            transform.position = transform.parent.position + Vector3.up * height;
        }
    }
}
