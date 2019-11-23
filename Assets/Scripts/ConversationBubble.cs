using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConversationBubble : MonoBehaviour
{
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Speak(string line)
    {
        text.text = line;
    }
}
