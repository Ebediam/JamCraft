using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : Interactable
{

    public NPCData data;

    public int conversationNumber = 0;

    public ConversationBubble conversationBubble;
    ConversationData conversation;

    public delegate void QuestDelegate();
    public QuestDelegate QuestCompletedEvent;

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
        player.LockMovement();
        
        conversationLine = 0;
        InteractionBubbleEnds();
        base.InteractionStarts();
        conversationBubble.gameObject.SetActive(true);

        if (!data.hasGreeted)
        {
            conversation = data.greetingsConversation;
            data.hasGreeted = true;

        }
        else if (data.hasQuest)
        {
            CheckQuestCondition();

            if (data.questFinished)
            {
                conversation = data.questCompletedConversation;
                data.hasQuest = false;
            }
            else
            {
                conversation = data.questNotCompletedConversation;
            }
        }
        else
        {
            conversation = data.exhaustedConversation;
        }

        NextLine();
        isSpeaking = true;
        player.InteractionEvent += NextLine;
        
    }

    public void NextLine()
    {       

        if(conversation.lines.Count <= conversationLine)
        {
            InteractionEnds();
            
        }
        else
        {
            conversationBubble.Speak(conversation.lines[conversationLine]);
            conversationLine++;
        }

    }
    public override void InteractionEnds()
    {
        base.InteractionEnds();
        conversationBubble.gameObject.SetActive(false);
        InteractionBubbleStarts();
        player.InteractionEvent -= NextLine;
        player.UnlockMovement();
    }

    public void CheckQuestCondition()
    {
        int requestItemNumber = 0;
        bool[] questCheck = new bool[data.itemRequest.Count];
        int playerItemNumber = 0;
        foreach (PickupData requestItem in data.itemRequest)
        {
            questCheck[requestItemNumber] = false;
            playerItemNumber = 0;
            foreach(PickupData playerItem in player.playerData.storedPickups)
            {
                
                if(requestItem == playerItem)
                {
                    if(data.itemAmount[requestItemNumber] <= playerItem.amount)
                    {
                        questCheck[requestItemNumber] = true;
                    }
                }
                playerItemNumber++;
            }
            requestItemNumber++;
        }

       for(int i=0; i<questCheck.Length; i++)
       {
            if (!questCheck[i])
            {
                return;
            }
       }

        data.questFinished = true;
        QuestCompletedEvent?.Invoke();

        requestItemNumber = 0;

        foreach (PickupData requestItem in data.itemRequest)
        {
            playerItemNumber = 0;
            foreach (PickupData playerItem in player.playerData.storedPickups)
            {
                if (requestItem == playerItem)
                {
                    playerItem.amount -= data.itemAmount[requestItemNumber];
                }
                playerItemNumber++;
            }
            requestItemNumber++;
        }
    

        if (data.rewardItem)
        {
            GameManager.GiveItemToPlayer(player, data.rewardItem);
        }

        GameManager.InventoryEvent?.Invoke();
    }
}
