using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject fullBridge;
    public GameObject unfinishedBridge;

    public NPC builder;

    // Start is called before the first frame update
    void Start()
    {
        builder.QuestCompletedEvent += CompleteBridge;
        fullBridge.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteBridge()
    {
        fullBridge.SetActive(true);
        unfinishedBridge.SetActive(false);
        builder.QuestCompletedEvent -= CompleteBridge;
    }
}
