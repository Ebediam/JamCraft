using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Talker : Interactable
{

    public ConversationData conversationData;
    public ConversationBubble conversationBubble;

    int conversationLine;

    bool isSpeaking;
    // Start is called before the first frame update
    void Start()
    {
        conversationBubble.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public override void InteractionBubbleStarts()
    {
        base.InteractionBubbleStarts();


    }

    public override void InteractionBubbleEnds()
    {
        base.InteractionBubbleEnds();

    }

    public override void InteractionStarts()
    {
        conversationLine = 0;
        InteractionBubbleEnds();
        base.InteractionStarts();
        conversationBubble.gameObject.SetActive(true);
        NextLine();
        isSpeaking = true;
        player.InteractionEvent += NextLine;
        

    }

    public void NextLine()
    {
        if(conversationData.lines.Count <= conversationLine)
        {
            InteractionEnds();
        }
        else
        {
            conversationBubble.Speak(conversationData.lines[conversationLine]);
            conversationLine++;
        }

    }
    public override void InteractionEnds()
    {
        base.InteractionEnds();
        conversationBubble.gameObject.SetActive(false);
        InteractionBubbleStarts();
        player.InteractionEvent -= NextLine;
    }
}
