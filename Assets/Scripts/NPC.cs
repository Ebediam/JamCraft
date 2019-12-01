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

    public GameObject buildStructure;
    public NPC nextPosition;

    // Start is called before the first frame update

    public  override void Awake()
    {
        Debug.Log("Override awake in "+data.name);

        if (buildStructure)
        {
            buildStructure.SetActive(false);
        }

        if (nextPosition)
        {
            nextPosition.gameObject.SetActive(false);
        }

        base.Awake();

    }
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

        if (nextPosition && data.questFinished)
        {
            data.hasGreeted = false;
            data.questFinished = false;
            data.hasQuest = true;
            GameManager.FadeInEvent += MoveNPC;
            GameManager.FadeOutEvent?.Invoke();
            ResetInteractable();
        }


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
            foreach (PickupData playerItem in player.playerData.storedPickups)
            {

                if (requestItem == playerItem)
                {
                    if (data.itemAmount[requestItemNumber] <= playerItem.amount)
                    {
                        questCheck[requestItemNumber] = true;
                    }
                }
                playerItemNumber++;
            }
            requestItemNumber++;
        }

        for (int i = 0; i < questCheck.Length; i++)
        {
            if (!questCheck[i])
            {
                return;
            }
        }

        QuestCompleted();
    }


    public void QuestCompleted()
    {

        data.questFinished = true;
        QuestCompletedEvent?.Invoke();
        if (buildStructure)
        {
            buildStructure.SetActive(true);
        }




        int requestItemNumber = 0;

        foreach (PickupData requestItem in data.itemRequest)
        {
            int playerItemNumber = 0;
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


        if (data.rewardItems.Count >0)
        {
            int i = 0;
            foreach(PickupData rewardItem in data.rewardItems)
            {
                GameManager.GiveItemToPlayer(player, rewardItem, data.rewardAmount[i]);
                i++;
            }
            
        }

        GameManager.InventoryEvent?.Invoke();
        
    }

    public void MoveNPC()
    {
        GameManager.FadeInEvent -= MoveNPC;
        if (nextPosition)
        {
            nextPosition.gameObject.SetActive(true);

        }

        gameObject.SetActive(false);


    }
}
