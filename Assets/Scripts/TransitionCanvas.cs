using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFadeOut()
    {
        gameManager.FadeIn();
    }
}
